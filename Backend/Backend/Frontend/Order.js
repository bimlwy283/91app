var checkBoxList = [];

function OrderObj(checkBox, orderId, price, cost, status) {
    this.checkBox = checkBox;
    this.orderId = orderId;
    this.price = price;
    this.cost = cost;
    this.status = status;
}

function ItemListObj(orderItem, price, cost, num) {
    this.orderItem = orderItem;
    this.price = price;
    this.cost = cost;
    this.num = num;
}

function renderOrder() {
    parent.$.ajax({
        type: "get",
        url: "http://localhost/91app/api/Order/listOrders",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            let count = 0;
            data.forEach((value) => {
                count++;
                var tmp;
                var checkBoxHTML = `<input type="checkbox" id="checkBox${count}" name="checkBox" onchange="confirmCheckBoxList(event,'${value.orderId}')">`
                var linkHTML = `<a href="ItemDetail.html?orderId=${value.orderId}">${value.orderId}</a>`
                tmp = new OrderObj(checkBoxHTML, linkHTML, value.price, value.cost, value.status);
                ftRefreshRows(tmp);
            })
        },
        error: function (exception) {
        }
    })
}

function ftRefreshRows(obj) {
    var jsonStr = JSON.stringify(obj);
    var jsonize = JSON.parse(jsonStr);
    var tmpArr = new Array();
    tmpArr.push(jsonize);
    var jsonStr_tmpArr = JSON.stringify(tmpArr);
    var json_tmpArr = JSON.parse(jsonStr_tmpArr);
    ft.loadRows(json_tmpArr, true);
}

function initOrder() {
    ft = FooTable.init('#tbResult', {
        "columns": [
            {
                "name": "checkBox",
                "title": "Check",
                "type": "html",
                "style": {
                    "width": 70
                }
            },
            {
                "name": "orderId",
                "title": "Order Id",
                "type": "html"
            },
            {
                "name": "price",
                "title": "Price",
                "breakpoints": "xs"
            },
            {
                "name": "cost",
                "title": "Cost",
                "breakpoints": "xs"
            },
            {
                "name": "status",
                "title": "Status",
                "breakpoints": "xs"
            }
        ],
        "paging": {
            "size": 5,
            "limit": 3,
        },
        "editing": {
            enabled: true,
            alwaysShow: true,
            allowAdd: false,
            allowEdit: false,
            allowDelete: false,
        }
    });
}

function removeAllDetail() {
    $("#tbResult tr").not(":first").remove();
    initOrder();
}

function confirmStatus() {
    parent.$.ajax({
        type: "put",
        url: "http://localhost/91app/api/Order/changeStatus",
        data: JSON.stringify(checkBoxList),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            alert(data);
            removeAllDetail();
            renderOrder();
        },
        error: function (jqXHR, exception) {
        }
    })
}

function confirmCheckBoxList(event, orderId) {
    if (event.target.checked) {
        checkBoxList.push(orderId);
    } else {
        checkBoxList = checkBoxList.filter((selectedId) => selectedId != orderId);
    };
}

function initItems() {
    ft = FooTable.init('#ItemResult', {
        "columns": [
            {
                "name": "orderItem",
                "title": "Order Item",
            },
            {
                "name": "price",
                "title": "Price",
                "breakpoints": "xs"
            },
            {
                "name": "cost",
                "title": "Cost",
                "breakpoints": "xs"
            },
            {
                "name": "num",
                "title": "Number of items",
                "breakpoints": "xs"
            }
        ],
        "paging": {
            "size": 5,
            "limit": 3,
        },
        "editing": {
            enabled: true,
            alwaysShow: true,
            allowAdd: false,
            allowEdit: false,
            allowDelete: false,
        }
    });
}

function renderItems() {
    var url = new URL(document.URL);
    let orderId = url.searchParams.get('orderId');
    $("#OrderId").text(`Order ID: ${orderId}`);
    parent.$.ajax({
        type: "get",
        url: "http://localhost/91app/api/Order/listItems?orderId=" + orderId,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            data.forEach((value) => {
                var tmp;
                tmp = new ItemListObj(value.itemName, value.price, value.cost, value.num);
                ftRefreshRows(tmp);
            })
        },
        error: function (exception) {
        }
    })
}