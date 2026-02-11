function toggleEdit(row, button) {
    const isEditMode = button.innerText === "Edit";
    const edits = row.querySelectorAll('.edit');
    const views = row.querySelectorAll('.view');
    const deleteBtn = row.querySelector('.btn-delete');

    edits.forEach(el => el.classList.toggle('d-none', !isEditMode));
    views.forEach(el => el.classList.toggle('d-none', isEditMode));

    button.innerText = isEditMode ? "Save" : "Edit";
    button.classList.toggle('btn-success', isEditMode);
    button.classList.toggle('btn-primary', !isEditMode);

    deleteBtn.innerText = isEditMode ? "Cancel" : "Delete";
    deleteBtn.classList.toggle('btn-secondary', isEditMode);
    deleteBtn.classList.toggle('btn-danger', !isEditMode);

    if (!isEditMode) saveChanges(row, button, deleteBtn);
}

function formatDateForServer(dateString) {
    const parts = dateString.split('.');
    if (parts.length === 3) {
        return `${parts[2]}-${parts[1]}-${parts[0]}`;
    }
    return dateString;
}

async function saveChanges(row, saveBtn, cancelBtn) {
    const id = row.dataset.id;

    const data = {
        id,
        name: row.querySelector('[data-field="Name"] input').value.trim(),
        dateOfBirth: row.querySelector('[data-field="DateOfBirth"] input').value,
        married: row.querySelector('[data-field="Married"] input').checked,
        phone: row.querySelector('[data-field="Phone"] input').value.trim(),
        salary: parseFloat(row.querySelector('[data-field="Salary"] input').value)
    };

    saveBtn.disabled = true;

    const response = await fetch('/Contact/UpdateContact', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });

    saveBtn.disabled = false;

    if (!response.ok) {
        alert("Error saving data");
        return;
    }

    row.querySelector('[data-field="Name"] .view').innerText = data.name;
    row.querySelector('[data-field="DateOfBirth"] .view').innerText = data.dateOfBirth;
    row.querySelector('[data-field="Married"] .view').innerText = data.married;
    row.querySelector('[data-field="Phone"] .view').innerText = data.phone;
    row.querySelector('[data-field="Salary"] .view').innerText = data.salary;

    row.style.backgroundColor = '#d4edda';
    setTimeout(() => row.style.backgroundColor = '', 800);
}
function handleDelete(row, button) {
    if (button.innerText === "Cancel") {
        toggleEdit(row, row.querySelector('.btn-edit'));
        return;
    }

    if (confirm("Delete this contact?")) {
        deleteContact(row.dataset.id, row);
    }
}

async function deleteContact(id, row) {
    const response = await fetch(`/Contact/DeleteContact?id=${id}`, { method: 'DELETE' });
    if (response.ok) row.remove();
}

document.getElementById('tableSearch').addEventListener('keyup', function () {
    let filter = this.value.toLowerCase();
    let rows = document.querySelectorAll('#tableBody tr');

    rows.forEach(row => {
        let text = row.textContent.toLowerCase();
        row.style.display = text.includes(filter) ? '' : 'none';
    });
});
function sortTable(columnIndex) {
    const table = document.getElementById("contactsTable");
    let rows = Array.from(table.rows).slice(1);
    let ascending = table.getAttribute("data-sort-dir") === "asc";

    rows.sort((rowA, rowB) => {
        let cellA = rowA.cells[columnIndex].innerText.toLowerCase();
        let cellB = rowB.cells[columnIndex].innerText.toLowerCase();

        if (!isNaN(parseFloat(cellA)) && !isNaN(parseFloat(cellB))) {
            return ascending ? cellA - cellB : cellB - cellA;
        }

        return ascending
            ? cellA.localeCompare(cellB)
            : cellB.localeCompare(cellA);
    });

    const tbody = table.querySelector("tbody");
    tbody.append(...rows);
    table.setAttribute("data-sort-dir", ascending ? "desc" : "asc");
}

document.getElementById('contactsTable').addEventListener('click', e => {
    const editBtn = e.target.closest('.btn-edit');
    const deleteBtn = e.target.closest('.btn-delete');
    const row = e.target.closest('tr');

    if (!row) return;

    if (editBtn) toggleEdit(row, editBtn);
    if (deleteBtn) handleDelete(row, deleteBtn);
});
