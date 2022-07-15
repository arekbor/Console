const connection = new signalR.HubConnectionBuilder()
    .withUrl("/console")
    .build();

let _connectionId = '';

connection.start()
    .then(function (){
       connection.invoke('getConnectionId')
           .then(function (connectionId){
              _connectionId = connectionId;
               joinRoom();
           });
    })
    .catch(function (){
       console.log('Failed to get connection id'); 
    });

    connection.on('RecieveTerminal',function (data){
    console.log(data);
    
    const rowMain = document.querySelector('.row-messages');
    
    //make row for whole msg
    const rowMsg = document.createElement('div');
    rowMsg.classList.add('row-msg');
    
    //make name element
    const nameElement = document.createElement('span');
    nameElement.setAttribute('id','data-receive-name');
    nameElement.appendChild(document.createTextNode(data.username));
    
    //make timestamp element
    const timeStampElement = document.createElement('span');
    timeStampElement.setAttribute('id','data-receive-timestamp');
    timeStampElement.appendChild(document.createTextNode(data.timeStamp));

    //make id element
    const idElement = document.createElement('span');
    idElement.setAttribute('id','data-receive-id');
    idElement.appendChild(document.createTextNode(data.userId));

    //make text element
    const textElement = document.createElement('span');
    textElement.setAttribute('id','data-receive-text');
    textElement.appendChild(document.createTextNode(data.text));

    //append all element to main div
    rowMsg.appendChild(nameElement);
    rowMsg.appendChild(idElement);
    rowMsg.appendChild(timeStampElement);
    rowMsg.appendChild(textElement);
    rowMain.appendChild(rowMsg);
});

let joinRoom = function (){
    const url = '/Terminal/JoinRoom/'+_connectionId;
    axios.post(url,null)
        .then(() => {
            console.log('Room joined');
        })
        .catch(() =>{
           console.log('Failed to join room!'); 
        });
}

let leaveRoom = function (){
    const url = '/Terminal/LeaveRoom/'+_connectionId;
    axios.post(url,null)
        .then(() =>{
            console.log('Room leaved');
        })
        .catch(() => {
            console.log('Failed to leave room!'); 
        });
}

let sendText = function (text){
    const formData = new FormData();
    formData.append('text',text);

    axios.post('Terminal/Send',formData)
        .then(() => {
           console.log('Text sent: ', text); 
        })
        .catch(() => {
           console.log('Failed to send text'); 
        });
}