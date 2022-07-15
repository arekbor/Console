let arrayOfLetters = [];
let caret = document.querySelector('#data-send-caret');

let allowedLetters = 
[
    'Digit1','Digit2','Digit3','Digit4','Digit5','Digit6','Digit7','Digit8','Digit9','Digit0','Minus','Equal',
    'KeyQ','KeyW','KeyE','KeyR','KeyT','KeyY','KeyU','KeyI','KeyO','KeyP','BracketLeft','BracketRight','Backslash',
    'KeyA','KeyS','KeyD','KeyF','KeyG','KeyH','KeyJ','KeyK','KeyL','Semicolon','Quote',
    'KeyZ','KeyX','KeyC','KeyV','KeyB','KeyN','KeyM','Comma','Period','Slash'
];

const processMsg = document.querySelector('#data-send-text');
document.addEventListener('keydown', function (e){
    //scroll to bottom
    const rowMain = document.querySelector('.row-main');
    
    rowMain.scrollTop  = rowMain.scrollHeight;
    //check if letter is allowed
    if(allowedLetters.includes(e.code)){
        arrayOfLetters.push(e.key);
    }
    //detect backspace
    if(e.code === 'Backspace' && arrayOfLetters.length > 0){
        arrayOfLetters.pop();
    }
    //detect space
    if(e.code === 'Space'){
        arrayOfLetters.push('\u00a0');
    }
    //clear array
    if(e.code === 'Delete' && arrayOfLetters.length > 0){
        arrayOfLetters = [];
    }
    //send message
    if(e.code === 'Enter' && arrayOfLetters.length > 0){
        sendText(arrayOfLetters.join(''));
        arrayOfLetters = [];
    }
    processMsg.textContent = arrayOfLetters.join('');
});

window.setInterval(function (){
   if(caret.style.display === 'none'){
       caret.style.display = 'inline';
   }else{
       caret.style.display = 'none';
   }
},500);




