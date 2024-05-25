// Variabili per mantenere lo stato dell'ordinamento delle colonne
var sortOrder = "desc";
var priceSortOrder = "desc";

// Funzione per alternare l'ordine di ordinamento (ascendente/discendente) per una colonna specifica
function toggleSortOrder(columnIndex) {
    if (columnIndex === 0) {
        sortOrder = sortOrder === "asc" ? "desc" : "asc";
        sortTable(sortOrder, columnIndex);
    } else if (columnIndex === 2) {
        priceSortOrder = priceSortOrder === "asc" ? "desc" : "asc";
        sortTable(priceSortOrder, columnIndex);
    }
}

// Funzione per ordinare la tabella in base alla colonna specificata e all'ordine specificato
function sortTable(order, columnIndex) {
    var rows = Array.from(document.querySelectorAll("tbody tr"));
    var sortedRows = rows.sort((rowA, rowB) => {
        var valueA = getValue(rowA.cells[columnIndex]);
        var valueB = getValue(rowB.cells[columnIndex]);
        if (order === "asc") {
            return valueA.localeCompare(valueB, undefined, { numeric: true });
        } else {
            return valueB.localeCompare(valueA, undefined, { numeric: true });
        }
    });

    var tbody = document.querySelector("tbody");
    tbody.innerHTML = "";
    sortedRows.forEach(row => tbody.appendChild(row));
}

// Funzione che restituisce il valore della cella, gestendo sia i valori di testo che i valori numerici (prezzi)
function getValue(cell) {
    if (cell.textContent.includes("€")) {
        return parseFloat(cell.textContent.replace(" €", ""));
    } else {
        return cell.textContent.trim().toLowerCase();
    }
}

// Funzione per visualizzare il modale di conferma per la cancellazione
function showDeleteModal(deleteUrl, pizzaName) {
    document.getElementById('pizzaNameToDelete').innerText = pizzaName;
    var deleteForm = document.getElementById('deleteForm');
    deleteForm.action = deleteUrl;

    var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}

// Funzione per chiudere l'alert di successo dopo 2 secondi
setTimeout(function () {
    var alert = document.getElementById('successAlert');
    var closeButton = document.getElementById('closeAlertButton');
    alert.classList.remove('show');
    closeButton.click();
}, 2000);

// Funzione per la ricerca dinamica delle pizze
function searchPizza() {
    var searchTerm = document.getElementById('searchInput').value.toLowerCase();
    var tableRows = document.querySelectorAll('#pizzaTable tbody tr');
    tableRows.forEach(function (row) {
        var nameCell = row.querySelector('td:first-child');
        var descriptionCell = row.querySelector('td:nth-child(2)');
        var name = nameCell.textContent.toLowerCase();
        var description = descriptionCell.textContent.toLowerCase();
        if (name.includes(searchTerm) || description.includes(searchTerm)) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
}
