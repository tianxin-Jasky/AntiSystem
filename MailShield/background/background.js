const onMessage = chrome.runtime.onMessage;
const sendMessage = chrome.runtime.sendMessage;
let isLoginPage = false;

function returnName(){
	return "Jsr";
}

function isLoginPage(){
	return isLoginPage;
}




onMessage.addListener(function(req, sender, sendResponse){

	let action = req.action;
	

	switch (action){
		case 'CHECK_PAGE':
			isLoginPage = req.isLoginPage;
			sendResponse('是初始界面');
			break;
		default:
			return;
	}
});