import { showNotification } from "./utils/notifications.js"

document.querySelectorAll(".ListDetails").forEach(element => {
    element.addEventListener("click", () => {

        document.getElementById("tableProductsContent").innerHTML = ""
        let { identificator } = element.dataset
        let responseRight
        fetch(`./Pedidos/Details/${identificator}`)
            .then(data => {
                responseRight = data.ok
                return data.json()
            })
            .then(({ message, data }) => {
                if (!responseRight) throw new Error(message)

                data.forEach(element => {
                    let { producto, price, quantity, total } = element
                    let trContent = `
                    <tr>
                    <td>${producto}</td>
                    <td>${price}</td>
                    <td>${quantity}</td>
                    <td>${total}</td>
                    </tr>
                    `
                    document.getElementById("tableProductsContent")
                        ?.insertAdjacentHTML("beforeend", trContent)
                })

            })
            .catch((message) => showNotification(message, "ERROR"))
    })
})

document.querySelectorAll(".removeElement").forEach(element => {
    element.addEventListener("click", () => {

        let { identificator } = element.dataset

        let successCall
        fetch(`/Pedidos/Delete/${identificator}`, {
            method: "DELETE",
            headers: {
                "content-Type": "application/json"
            }
        })
            .then(data => {
                successCall = data.ok
                return data.json()
            })
            .then(data => {
                if (!successCall) throw new Error(data.message)
                showNotification(data.message, "success")
                document.getElementById(`tr_${identificator}`).remove()
            })
            .catch(message => showNotification(message, "ERROR"))
    })
})