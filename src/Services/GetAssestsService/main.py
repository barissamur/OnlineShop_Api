import pika

# RabbitMQ sunucu ayarları
rabbitmq_host = "localhost"
queue_name = "test-message-queue"

# RabbitMQ'ya bağlantı kurma
connection = pika.BlockingConnection(pika.ConnectionParameters(host=rabbitmq_host))
channel = connection.channel()

# Belirli bir queue üzerinde mesajları dinlemek için queue'nun var olduğundan emin olun
channel.queue_declare(queue=queue_name, durable=True)


# Callback fonksiyonu, alınan mesajı işler
def callback(ch, method, properties, body):
    print(f" [x] Received {body}")


# Queue üzerinden mesajları almak için consumer ayarlama
channel.basic_consume(queue=queue_name, on_message_callback=callback, auto_ack=True)

print(" [*] Waiting for messages. To exit press CTRL+C")
channel.start_consuming()
