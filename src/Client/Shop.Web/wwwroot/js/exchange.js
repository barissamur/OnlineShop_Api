let selectExchange = $('#stockSelect')

const getExchange = (e) => {
    // Seçili option'un metnini almak için
    let selectedText = $(e.target).find('option:selected').text();


    // Fetch API kullanarak GET isteği
    fetch(`/home/getExchange/${selectedText}`)
        .then(response => {
            // Yanıtın başarılı olup olmadığını kontrol et
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

selectExchange.on('change', getExchange);

console.log(selectExchange);