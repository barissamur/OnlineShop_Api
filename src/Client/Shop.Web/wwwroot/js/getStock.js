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
            // Yanıtı JSON olarak dönüştür
            return response.json();
        })
        .then(data => {
            // Elde edilen veriyi işle
            console.log(data);
        })
        .catch(error => {
            // Hata durumunda hata mesajını göster
            console.error('Fetch Error:', error);
        });
}

selectStock.change(getStock);
