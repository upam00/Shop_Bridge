const uri = 'api/items';
let todos = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const ItemName = document.getElementById('ItemName').value;
    const About = document.getElementById('About').value;
    const Price = document.getElementById('Price').value;
    const ImageFile = document.getElementById('ImageFile');

    var formdata = new FormData();
    formdata.append("ItemName", ItemName);
    formdata.append("About", About);
    formdata.append("Price", Price)
    formdata.append("ImageFile", ImageFile.files[0], ImageFile.value)

    var requestOptions = {
        method: 'POST',
        body: formdata,
        redirect: 'follow'
    };



    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(() => {
            getItems();
            console.log("Post complete")
            document.getElementById('addForm').reset();
            //clear form
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        console.log(item)
        //let isCompleteCheckbox = document.createElement('input');
        //isCompleteCheckbox.type = 'checkbox';
        //isCompleteCheckbox.disabled = true;
        //isCompleteCheckbox.checked = item.isComplete;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let imgElem = document.createElement('img');
        imgElem.setAttribute('src', "data:image/jpg;base64," + item.imageBase64);


        let tr = tBody.insertRow();

        //let td1 = tr.insertCell(0);
        //td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(0);
        let textNode = document.createTextNode(item.itemName);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(1);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(2);
        td4.appendChild(deleteButton);

        let td5 = tr.insertCell(3);
        td5.appendChild(imgElem)
    });

    todos = data;
}