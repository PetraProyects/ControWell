
window.Graficar = async (crudo, nafta, agua) => {    
    const ctx = document.getElementById('myChart');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Crudo', 'Nafta', 'Agua'],
            datasets: [{
                label: 'BLS NETOS',
                data: [crudo, nafta, agua],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });    
}
window.GraficarVolTorta = async () => {
    const ctx = document.getElementById('Torta');

    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Red', 'Blue'],
            datasets: [{
                label: '# of Votes',
                data: [12, 19],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}





