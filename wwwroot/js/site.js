﻿const uri = 'api/items';
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
        .then(() => { console.log("Deleted"); getItems() } )
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);
    //console.log("ItemDisplay"+item.id)
    document.getElementById('edit-ID').value = item.id;
    document.getElementById('edit-ItemName').value = item.itemName;
    document.getElementById('edit-About').value = item.about;
    document.getElementById('edit-Price').value = item.price;

    //document.getElementById('edit-ItemImage').value = item.imageBase64;
    let imgElem = document.getElementById('edit-Image');
    imgElem.setAttribute('src', "data:image/jpg;base64," + item.imageBase64);

    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {

    const Id = document.getElementById('edit-Id').value;
    const ItemName = document.getElementById('edit-ItemName').value;
    const About = document.getElementById('edit-About').value;
    const Price = document.getElementById('edit-Price').value;
    const ImageFile = document.getElementById('edit-ImageFile');

    var formdata = new FormData();
    if (Id != "NoId")
        formdata.append("Id", Id)
    
    formdata.append("ItemName", ItemName);
    formdata.append("About", About);
    formdata.append("Price", Price)
    formdata.append("ImageFile", ImageFile.files[0], ImageFile.value)

    //console.log(formdata)
    if (Id == "NoId") {
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
                //document.getElementById('addForm').reset();
                //clear form
            })
            .catch(error => console.error('Unable to add item.', error));
        $('#exampleModal').modal('toggle');

    }
    else {
        var requestOptions = {
            method: 'PUT',
            body: formdata,
            redirect: 'follow'
        };


        //.then(response => response.json())
        fetch(uri + "/" + Id, requestOptions)
            .then(() => {
                getItems();
                console.log("Updated")
                document.getElementById('edit-form').reset();
                //document.getElementById('edit-Image').removeAttribute('src')
                //clear form
            })
            .catch(error => console.error('Unable to add item.', error));

        $('#exampleModal').modal('toggle');
    }

    /*const itemId = document.getElementById('edit-id').value;
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
    */
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('ItemList');
    tBody.innerHTML = '';

    console.log(data)
    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        //console.log(item)
        //let isCompleteCheckbox = document.createElement('input');
        //isCompleteCheckbox.type = 'checkbox';
        //isCompleteCheckbox.disabled = true;
        //isCompleteCheckbox.checked = item.isComplete;

        const ID = item.id;
        //console.log(ID);

        //-----
        /*
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        //editButton.setAttribute("onClick", "displayEditForm("+46328523+")");
        editButton.addEventListener("click", function () { displayEditForm(ID) })

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.addEventListener("click", function(){ deleteItem(ID) })
        //deleteButton.setAttribute("onClick", "deleteItem(6109bf63335419ed0e4a83cc)");

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
        */
        //---------------


        //var card = document.getElementsByTagName("sampleCard")
        //console.log(card)
        //var newCard = card.cloneNode(true)
        //card.getElementsByTagName("ItemName").value = item.itemName
        //tBody.appendChild(newCard)

        var code = "";
        //code += "                    <div>";
        code += "                        <div class=\"card mb-4 box-shadow\">";
        code += "                            <img class=\"card-img-top\" src=\"data:image/jpg;base64," + item.imageBase64+ "\" alt=\"Card image cap\">";
        code += "                            <div class=\"card-body\">";
        code += "                                <h2 class=\"card-text\" id=\"ItemName\">" + item.itemName + "</h2>";
        code += "                                 <p class=\"card-text\" >" + item.about+"</p>"                      
        code += "                                <div class=\"d-flex justify-content-between align-items-center\">";
        //code += "                                    <div class=\"btn-group\">";
        code += "                                        <button type=\"button\" class=\"btn btn-sm btn-outline-secondary\" data-toggle=\"modal\" data-target=\"#exampleModal\" data-id=\"" + ID + "\" data-ItemName=\"" + item.itemName + "\" data-About=\"" + item.about + "\" data-Price=\"" + item.price + "\"> Update</button > ";
        code += "                                        <button type=\"button\" class=\"btn btn-sm btn-outline-danger\" id=\"del-" + ID+ "\">Delete</button>";
        code += "                                    </div>";
        //code += "                                    <small class=\"text-muted\">9 mins</small>";
        code += "                                </div>";
        code += "                            </div>";
        code += "                        </div>";
       // code += "                    </div>";
        code += " ";

        const html = document.createElement('div')
        html.classList.add("col-md-4")
        html.innerHTML = code;

        tBody.appendChild(html)
        //tBody.insertAdjacentElement('beforebegin', html)

        const id = "del-" + ID;
        const elem = document.getElementById(id);
        //console.log(elem)
        elem.addEventListener("click", function () { deleteItem(ID) })

        //onClick=\"(function () {deleteItem(" + 3465464564 + ");}())
        /*
    alert('Hey i am calling');
    return false;
})();return false;
         * 
         * */

    });

    todos = data;
}