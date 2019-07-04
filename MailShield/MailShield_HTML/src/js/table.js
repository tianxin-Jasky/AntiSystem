var data = [];
let storageobj={};

//获取邮箱的邮件数量
storageobj = JSON.parse(localStorage.getItem("!！@@##¥"));
var mailnumber =storageobj.MailNumber;


for(var t = mailnumber-1; t>=0 ; t--){
    storageobj = JSON.parse(localStorage.getItem(t));

    data[data.length] =  {title:storageobj.Mailsubject, mailbox: storageobj.Mailsender, date: storageobj.Mailtime, result:storageobj.Mailjudge};
}
for(var i=0;i<mailnumber;i++){
    localStorage.removeItem(i);
}

//把邮箱的删除，避免出现问题。
localStorage.removeItem("!！@@##¥");

var Mail = function (title, mailbox, date, result)
{
    this.title = title;
    this.mailbox = mailbox;
    this.date = date;
    this.result = result;
}
 
var mails = [];

for (var i = 0; i < data.length; i++)
{
    var d = data[i];
    mails.push(new Mail (d["title"], d["mailbox"], d["date"], d["result"]));
}

onload = function ()
{
    var table = document.createElement("table");
    var tbody = document.createElement("tbody");
    table.appendChild(tbody);
    var caption = table.createCaption();
    var tr = tbody.insertRow (0);
    var str = "邮件标题,发送邮箱,发送日期,判断结果".split(",");
    for (var i = 0; i < str.length; i++)
    {
        var th = document.createElement("th");
        th.innerHTML = str[i];
        tr.appendChild (th);
    }
    for (var i = 0; i < mails.length; i++)
    {
        var tr = tbody.insertRow (tbody.rows.length);
        var obj = mails[i];
        for (var p in obj)
        {
            var op = obj[p];
            var td = tr.insertCell (tr.cells.length);
            td.innerHTML = op;
        }
    }
    document.body.appendChild(table);
    $("tr td").each(function(){
        $(this).html("<span>" + $(this).html() + "</span>")
    })
}
