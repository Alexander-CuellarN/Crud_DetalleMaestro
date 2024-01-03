
import { showNotification } from "./utils/notifications.js"

let ProductsList
fetch("/Producto/GellAllProducts")
    .then(data => data.json())
    .then(({ data }) => ProductsList = data)
    .catch(_ => showNotification("Ha ocurrido un error listando los productos.", "ERROR"))

const refreshEvents = () => {
    document.querySelectorAll(".tr_product_emmit")?.forEach(item => item.addEventListener("change", () => changeProductHandler(item)))
    document.querySelectorAll(".tr_cant_emmit")?.forEach(item => item.addEventListener("change", () => changeQuantityHandler(item)))
    document.querySelectorAll(".tr_delete_row")?.forEach(item => item.addEventListener("click", () => DeleteHandler(item)))
}

const AddTr = (index, producto) => {
    let template =
        `<tr id="tr_${index}">
            <td>
                <select name="Product[]" class="form-control tr_product_emmit" data-tr=${index} id="tr_select_${index}" required>
                <option value="-1" > Selecione un item </option>
                ${(producto.map(item => { return "<option value='" + item.idProducto + "'> " + item.nombre + "    </option>" }))
        }    
                </select>
            </td>
            <td>
                <input class="form-control" type="text" value="" data-tr=${index} id="tr_prize_${index}" disabled>
            </td>
           <td>
                <input class="form-control" type="text" value="" disabled id="tr_stock_${index}" disabled>
            </td>
            <td>
                <input class="form-control tr_cant_emmit" type="text" name="cantidadProdct[]" data-tr=${index} id="tr_cant_${index}" required>
            </td>
            
            <td> 
                <input class="form-control tr_total_to_sum" type="text" value="" disabled id="tr_prize_total_${index}">
            </td>
            <td>
                <buttom type="buttom" class="btn btn myBtn myBtnInfo tr_delete_row" data-tr=${index} id="tr_delete_${index}" > Eliminar</buttom>
            </td>
        </tr>`;

    document.querySelector("#tableProducts > tbody").insertAdjacentHTML("beforeend", template);
    refreshEvents();
}

let counter = document.getElementById("counterRowCurrent")?.value || 0

document.getElementById("addProduct").addEventListener("click", () => {
    AddTr(counter, ProductsList)
    counter++
})

const changeProductHandler = (item) => {

    let { value: selectedItem } = item.options[item.selectedIndex]
    let { tr } = item.dataset

    let tr_prize = document.getElementById(`tr_prize_${tr}`)
    let tr_Stock = document.getElementById(`tr_stock_${tr}`)
    if (tr === '-1') {
        tr_prize.value = ""
        document.getElementById(` tr_cant_${tr}`).value = ""
        document.getElementById(`tr_prize_total_${tr}`).value = ""
        tr_Stock.value = ""
        return
    }

    let product = ProductsList.find(p => p.idProducto == selectedItem)
    tr_prize.value = product.precio
    tr_Stock.value = product.stock

}
const updateTotalPedido = () => {
    let totalCurrent = 0
    document.querySelectorAll(".tr_total_to_sum").forEach(element => {
        totalCurrent += Number(element.value)
    })

    document.getElementById("CurrentTotalPedido").value = totalCurrent
}

const changeQuantityHandler = (item) => {

    let { dataset: { tr }, value } = item
    document.getElementById(`tr_prize_total_${tr}`).value = 0
    let prize = document.getElementById(`tr_prize_${tr}`).value
    document.getElementById(`tr_prize_total_${tr}`).value = Number(prize) * Number(value)
    updateTotalPedido()
}
const DeleteHandler = (item) => {
    let { dataset: { tr } } = item
    document.getElementById(`tr_${tr}`).remove();
    updateTotalPedido()
}
document.getElementById("addNewPedido").addEventListener("click", () => {

    let Pedido = {
        Nit: document.getElementById("txtNit").value,
        Nombre: document.getElementById("txtNombre").value,
        Total: Number(document.getElementById("CurrentTotalPedido").value)
    }

    let products = [];
    let Quantities = [];

    document.querySelectorAll(".tr_product_emmit").forEach(element => { products.push(Number(element.value)) })
    document.querySelectorAll(".tr_cant_emmit").forEach(element => { Quantities.push(Number(element.value)) })

    let data = {
        pedido: Pedido,
        productos: products,
        productoQuantity: Quantities
    }

    let idPedido = document.getElementById("UpdateNewPedido")?.value || 0
    let endPont
    if (idPedido === 0) {
        endPont = "/Pedidos/Create";
    } else {
        endPont = `/Pedidos/Edit/${idPedido}`
        data.pedido.idPedido = idPedido
    }

    let goodResponse

    fetch(endPont, {
        method: "POST",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify(data)
    })
        .then(data => {
            goodResponse = data.ok
            return data.json()
        })
        .then(({ message }) => {
            if (!goodResponse) throw new Error(message)

            showNotification(message, "success")
            setTimeout(() => { window.location.href = "/Pedidos" }, 1000)
        })
        .catch((message) => showNotification(message || "Se ha producido un error", "ERROR"))
})

refreshEvents()