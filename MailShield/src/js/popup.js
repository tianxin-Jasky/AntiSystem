const find = function(selector) {
	return document.querySelector(selector);
}

const sendMessage = chrome.tabs.sendMessage;
const onMessage = chrome.runtime.onMessage;
const query = chrome.tabs.query;

const container = find('#container');
const showAddBtn = find('#showAddBtn');
const addArea = find('.add-area');
const cancelBtn = find('#cancelBtn');
const confirmBtn = find('#confirmBtn');

const accountBtnArea = find('#accountBtnArea');

const accountNameInput = find('#accountName');
const accountInput = find('#account');
const passwordInput = find('#password');
//用于连接服务器。
var socket;


// var loginjudge=true;
var Wrongmail;

const Plugin = {

	initButton: function() {
		let that = this;
		let str = '';
		let obj = {};
		let account = '';
		let password = '';
		// 遍历 localStorage ，生成按钮
		for (let key in localStorage) {
			//显示每个btn的key值作为标识，然后重新创建一个btn
			if (localStorage.hasOwnProperty(key)) {

				str = that.getStorage(key);

				obj = JSON.parse(str);

				account = obj.account;
				password = obj.password;

				that.createButton(key);
			}
		}
	},


	bindEvent: function() {

		let that = this;

		// 对 account-btn 和 delete-btn 做事件代理
		accountBtnArea.addEventListener('click', function(e){
			e.stopPropagation();
			//e.target是一个dom对象
			let target = e.target;
			let className = e.target.className;
			let key = '';
			let storageObj = {};

			if (className === 'account-btn'){

				//target.parentNode.getAttribute('storage-key');是为了获取对应元素的值
				key = target.parentNode.getAttribute('storage-key');
				Wrongmail=key;
				storageObj = JSON.parse(that.getStorage(key));

				// // 填充页面的 input
				// key = target.parentNode.getAttribute('storage-key');
				// storageObj = JSON.parse(that.getStorage(key));
				// that.fillTheBlank(storageObj.account, storageObj.password);

				//调用服务器的连接，并传输相应的account和password
				that.sendMsg(storageObj.account,storageObj.password);

				// //这里如果邮箱登陆失败，就会删除
				// if (!loginjudge){
				// 	that.removeStorage(key);
				// 	loginjudge=true;
				// }
			} else if (className === 'fa fa-trash'){

				// 删除当前项
				key = target.parentNode.parentNode.getAttribute('storage-key');
				that.removeStorage(key);
				location.reload();
			} else {
				console.log('neither account-btn nor delete-btn.');
			}

		}, false);


		//添加按钮  展示输入面板
		showAddBtn.addEventListener('click', function() {
			addArea.style.display = 'block';
			showAddBtn.style.display = 'none';
		}, false);

		//取消按钮  隐藏输入面板
		cancelBtn.addEventListener('click', function() {
			// addArea.style.display = 'none';
			// showAddBtn.style.display = 'block';
			location.reload();
		}, false);

		//确认按钮 传输三个text并直接保存下来
		confirmBtn.addEventListener('click', function() {

			let accountName = accountNameInput.value;
			let account = accountInput.value;
			let password = passwordInput.value;

			// empty check
			if (accountName === '' || account === '' || password === '') {
				console.info('input incomplete.');
				return;
			}


			let _accountObj = {
				account: account,
				password: password
			};

			let _accountStr = JSON.stringify(_accountObj);

			that.setStorage(accountName, _accountStr);

			that.clearInput();
			location.reload();

		}, false);

	},

	//对于java服务器的连接
	connectServer:function(){
		//本地的ip地址
		var socket_ip="192.168.153.245";
		//var socket_ip="127.0.0.1";

		let that = this;

		socket= new WebSocket('ws://'+socket_ip+':1234');

		console.log("连接服务开始！");
		socket.onopen = function(event)
		{
			console.log("连接服务成功！");
		};
		console.log("连接服务结束！");
		var mail_subject="";
		var mail_time="";
		var mail_sender=""
		var mail_judge=""
		//在localstorage中存储的key值
		var mail_Count=-1;
		// 监听消息
		socket.onmessage = function(event)
		{
			//console.log('Client received a message',event);

			//这是分割的
			var str = event.data.split('!@#%&');

			if(parseInt(str[0]) != -1){
				//邮箱登陆成功
				// loginjudge=true;

				//将邮箱的数量保存下来
				let MailObj = {
					MailNumber:parseInt(str[0])
				};
				let  mail_number = JSON.stringify(MailObj);
				var mai_key = "!！@@##¥";
				that.setStorage(mai_key,mail_number);



				//console.log("邮箱的数量：",JSON.parse(localStorage.getItem("!！@@##¥")).MailNumber);

				for(var i=1,j=0;i<str.length;i++,j++){
					var ramainder = j%4;
					switch (ramainder) {
						case 0:
							mail_subject = str[i];
							break;
							//这里不会去考虑 缺少某个数据的情况
						case 1:
							 mail_time = str[i];
							 break;
						case 2:
							mail_sender = str[i];
							break;
						case 3:
							mail_judge = str[i];
							//TODO:在这里去调用跳转界面的函数，用上面已经有的三个元素去给他赋值
							mail_Count++;

							let subject =mail_subject;
							let time=mail_time;
							let sender=mail_sender;
							let judge=mail_judge;

							let mailObj = {
								Mailsubject:subject,
								Mailtime:time,
								Mailsender:sender,
								Mailjudge:judge
							};
							let mailStr = JSON.stringify(mailObj);
							that.setStorage(mail_Count,mailStr);
							break;
					}
				}
				//调用那边的html
				window.location = "/MailShield_HTML/result.html";
			}else {
				// loginjudge=false;
				that.removeStorage(Wrongmail)
				window.location = "/MailShield_HTML/retry.html";
				//这里如果邮箱登陆失败，就会删除
			}


		};

		// 监听Socket的关闭
		socket.onclose = function(event)
		{
			console.log('服务器的关闭')
		};

		socket.onerror = function(event) {
			//alert('无法连接到:' + socket_ip);
			console.log('服务器的连接失败')
		};

	},

	//发送客户机account和password
	sendMsg:function(account,password){
			var message = account + "!@#%&"+password;
			socket.send(message);
			// socket.send(password);
		},

	//显示btnList的界面
	showPanel: function() {
		addArea.style.display = 'block';
		showAddBtn.style.display = 'none';
	},

	//隐藏btnList的界面
	hidePanel: function() {
		addArea.style.display = 'none';
		showAddBtn.style.display = 'block';
	},


	// 存
	setStorage: function(key, value) {
		localStorage.setItem(key, value);
	},

	// 读
	getStorage: function(key) {
		let _value = localStorage.getItem(key);
		return _value;
	},

	// 删
	removeStorage: function(key) {
		localStorage.removeItem(key);
	},

	// 清空 input
	clearInput: function() {
		accountNameInput.value = '';
		accountInput.value = '';
		passwordInput.value = '';
	},

	// 在 showAddButton 之中加入一个 button
	createButton: function(wording, account, password) {

		// button-row
		let div = document.createElement('div');
		div.className = 'button-row';
		div.setAttribute('storage-key', wording);

		// account-btn
		let btn = document.createElement('button');
		btn.className = 'account-btn';

		btn.innerText = wording;

		// icon-container
		let iconDiv = document.createElement('div');
		iconDiv.className = 'icon-container';

		// icon
		let icon = document.createElement('i');
		icon.className = 'fa fa-trash';

		//在图标上添加子图标
		iconDiv.appendChild(icon);

		//在div中添加子类
		div.appendChild(btn);
		div.appendChild(iconDiv);

		//添加
		accountBtnArea.appendChild(div);
	},

	// 发送指令到 content_script，填充页面的 input
	fillTheBlank: function(account, password){
		query({
				active: true,
				currentWindow: true
			}, function(tabs) {
				sendMessage(tabs[0].id, {
					action: 'FILL_THE_BLANK',
					account: account,
					password: password
				}, function(res) {
					console.log(res);
			});
		});
	},

	init: function() {
		this.initButton();
		//一开始初始化的时候，就可以连接服务器了
		this.connectServer();
		this.bindEvent();
	}

};

Plugin.init();
