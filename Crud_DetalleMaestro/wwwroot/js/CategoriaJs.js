
import { showNotification  } from '../js/utils/notifications.js'

document.addEventListener('show.bs.modal', event => {
    let { bsAction: Action, bsCategoria: idCategoria } = event.explicitOriginalTarget.dataset

    document.querySelectorAll(".textAction").forEach(element => {
        element.innerHTML = ` ${Action} `
        element.classList.remove("myBtnEdit")
        element.classList.remove("myBtnNew")

        element.classList.add((idCategoria !== '0') ? 'myBtnEdit' : 'myBtnNew')
    })

    if (idCategoria !== '0') {

        document.getElementById("ModalIdCategoria").value = idCategoria

        fetch(`./Categoria/GetCategoriaById/${idCategoria}`)
            .then(data => data.json())
            .then(data => document.getElementById("txtModalName").value = data.nombre)
            .catch(_ => alert("Se ha produccido un error"))
    }
});

document.getElementById("ModalActionConfirm").addEventListener("click", () => {
    let value = document.getElementById("txtModalName").value
    let Action = document.querySelector("#ModalActionConfirm > span").innerHTML

    if (value === '')
        return

    if (Action.trim() === "Crear") {
        AddCategoria(value)
    } else {
        UpdateCategoria(value)
    }
})

const listarCategorias = async () => {

    let request = await fetch("/Categoria/ListAll");
    let data = await request.json();
};



const addTr = (idCategoria, nombre) => {

    let plantilla = `
                   <tr id="tr_${idCategoria}">
                        <td>${nombre}</td>
                        <td>
                            <button class="btn myBtn myBtnEdit"
                                id="addCategoria"
                                data-bs-toggle="modal"
                                data-bs-target="#modelCategoriaActions"
                                data-bs-action="Modificar"
                                data-bs-categoria="${idCategoria}">
                                Editar
                             </button>

                            <button data-indenficador="${idCategoria}" 
                            class="btn myBtn myBtnDelete 
                            deleteAction">Borrar</button>
                         </td>
                 </tr>`;

    document.getElementById("TableCategoriaList").innerHTML += plantilla

    document.querySelectorAll(".deleteAction").forEach(item => {
        item.addEventListener("click",() => DeleteRow(item))
    })
}

const AddCategoria = async (value) => {
    let request = await fetch("/Categoria/Create", {
        method: "Post",
        headers: {
            "content-type": "application/json"
        },
        body: JSON.stringify({ Nombre: value })
    });

    if (!request.ok) {
        showNotification("No se ha podido crear el objecto", "ERROR");
        return
    };

    let { response, categoriaViewModel: { idCategoria, nombre } } = await request.json();

    addTr(idCategoria, nombre);
    document.getElementById("txtModalName").value = ""
    document.getElementById("ModalCloseElement").click()
    showNotification(response, "Success");
}

const UpdateCategoria = (value) => {

    let id = document.getElementById("ModalIdCategoria").value

    let data = {
        IdCategria: id,
        Nombre: value
    }
    fetch("/Categoria/Update", {
        method: "PUT",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify(data)
    })
        .then(data => data.json())
        .then(({ message }) => {
            console.log(`#tr_${id} td:first-child`)
            document.querySelector(`#tr_${id} td:first-child`).innerHTML = value
            document.getElementById("ModalIdCategoria").value = 0
            document.getElementById("txtModalName").value = ""
            document.getElementById("ModalCloseElement").click()
            showNotification(message, "Success")
        })

}

document.querySelectorAll(".deleteAction").forEach(item => {
    item.addEventListener("click", () => DeleteRow(item))
})

function DeleteRow(item) {
    let { indenficador } = item.dataset

    fetch(`/Categoria/Delete/${indenficador}`, {
        method: "DELETE",
    })
        .then(data => data.json())
        .then(({ message }) => {
            document.getElementById(`tr_${indenficador}`).remove()
            showNotification(message, "success")
        })
        .catch(_ => showNotification("No se ha podido eliminar la categoria", "Error"))
}