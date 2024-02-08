const selectStock = $('#stockSelect');

const getStock = () => {
    // Seçili option'un metnini doğrudan selectStock üzerinden almak
    const selectedText = selectStock.find('option:selected').text();

    // Fetch API kullanarak POST isteği
    fetch('/home/getStock', {
        method: 'POST', // İstek metodunu POST olarak belirle
        headers: {
            'Content-Type': 'application/json', // Gönderilen verinin tipini belirle
        },
        body: JSON.stringify({ stock: selectedText }) // Gönderilecek veriyi JSON formatında hazırla
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            return response.json();
        })
        .then(data => { 
        })
        .catch(error => {
            // Hata durumunda hata mesajını göster
            console.error('Fetch Error:', error);
        });
}

selectStock.on('change', getStock);

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/stockHub")
    .build();

connection.on("ReceiveMessage", function (message) {
    $('#showPrice').text(message);
    // Burada mesajı arayüzde göstermek için ilgili kodu yazabilirsiniz.
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});