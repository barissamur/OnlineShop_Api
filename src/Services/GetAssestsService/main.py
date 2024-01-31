import threading
from flask import Flask
import pika
import consul

app = Flask(__name__)


@app.route("/health")
def health_check():
    # Basit sağlık kontrolü
    return "OK", 200


def start_flask_app():
    app.run(host="0.0.0.0", port=5006, threaded=True)


def start_rabbitmq_consumer():
    # RabbitMQ sunucu ayarları
    rabbitmq_host = "localhost"
    connection = pika.BlockingConnection(pika.ConnectionParameters(host=rabbitmq_host))
    channel = connection.channel()
    exchange_name = "OnlineShop_Api.Messages:TestMessage"
    queue_name = "my_test_queue"
    channel.exchange_declare(
        exchange=exchange_name, exchange_type="fanout", durable=True
    )
    channel.queue_declare(queue=queue_name, durable=True)
    channel.queue_bind(exchange=exchange_name, queue=queue_name)

    def callback(ch, method, properties, body):
        print(f" [x] Received {body}")

    channel.basic_consume(queue=queue_name, on_message_callback=callback, auto_ack=True)
    print(" [*] Waiting for messages. To exit press CTRL+C")
    channel.start_consuming()


def deregister_service(service_id):
    c = consul.Consul()
    c.agent.service.deregister(service_id)


if __name__ == "__main__":
    # Flask uygulamasını ayrı bir thread'de başlat
    flask_thread = threading.Thread(target=start_flask_app)
    flask_thread.start()

    # Consul kaydı (Flask thread'i başladıktan sonra)
    c = consul.Consul()

    # Eski servis örneklerinin ID'lerini buraya yazın
    old_service_ids = ["PythonGetAssetes"]

    # Eski servis örneklerini deregister yap
    for service_id in old_service_ids:
        deregister_service(service_id)

    # Servisi kaydet
    c.agent.service.register(
        "PythonGetAssetesService",
        service_id="PythonGetAssetes",
        address="192.168.1.25",  # localhost yerine localhost kullanılıyor
        port=5006,
        check=consul.Check.http(
            "http://192.168.1.25:5006/health",  # Sağlık kontrolü için adres güncellendi
            interval="10s",
            timeout="5s",
        ),
    )

    # RabbitMQ tüketici döngüsünü başlat
    start_rabbitmq_consumer()
