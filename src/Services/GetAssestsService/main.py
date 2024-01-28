import pika

# RabbitMQ sunucu ayarları
rabbitmq_host = "localhost"

# Bağlantıyı kur
connection = pika.BlockingConnection(pika.ConnectionParameters(host=rabbitmq_host))
channel = connection.channel()

# Exchange ve Queue tanımlamaları
exchange_name = "OnlineShop_Api.Messages:TestMessage"
queue_name = "my_test_queue"  # Özelleştirilebilir bir kuyruk adı

# Exchange'i tanımla (varsa bu adımı atlar)
channel.exchange_declare(exchange=exchange_name, exchange_type="fanout", durable=True)

# Kuyruğu tanımla (varsa bu adımı atlar)
channel.queue_declare(queue=queue_name, durable=True)

# Kuyruğu Exchange'e bağla
channel.queue_bind(exchange=exchange_name, queue=queue_name)


# Mesajları işlemek için callback fonksiyonu
def callback(ch, method, properties, body):
    print(f" [x] Received {body}")


# Mesajları almak için consumer ayarla
channel.basic_consume(queue=queue_name, on_message_callback=callback, auto_ack=True)

# Mesajları dinlemeye başla
print(" [*] Waiting for messages. To exit press CTRL+C")
channel.start_consuming()
