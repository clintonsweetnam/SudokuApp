// Write your Javascript code.
var gameId;
var userId;

var socket;
var scheme = document.location.protocol === 'https:' ? 'wss' : 'ws';
var port = document.location.port ? ':' + document.location.port : '';

var connectionUrl = scheme + '://' + document.location.hostname + ':' + 5000;

function startNewGame() {
    $.get('api/game', function (data) {
        HandleConnection(data);
    });
}

function joinExistingGame() {
    $.get('api/game?gameId=' + $('#existingGameId').val(), function (data) {
        HandleConnection(data);
    });
}

function HandleConnection(data) {
    gameId = data.gameId;
    userId = data.userId;

    $('#gameId').text(data.gameId);
    $('#initializationButtons').hide();

    createGameContainer();

    socket = new WebSocket(connectionUrl);

    socket.onopen = function () {
        var message = {};
        message.Type = 'Connection';
        message.GameId = data.gameId;
        message.Content = JSON.stringify(data);
        socket.send(JSON.stringify(message));
    };

    socket.onmessage = function (evt) {
        var message = JSON.parse(evt.data);
        if (message.Type === 'Info')
            console.log(message.Content);
        else if (message.Type === 'GameReady') {
            var messageContent = JSON.parse(message.Content);
            window.setTimeout(function () {
                console.log('GOOOO!!');
            }, messageContent.StartInSeconds * 1000);
        }
        else if (message.Type === 'TileFocus') {
            console.log(message.Content);
            var tileFocusEvent = JSON.parse(message.Content);
            var tileId = '#' + tileFocusEvent.XPos + "-" + tileFocusEvent.YPos;
            if (tileFocusEvent.UserId === userId) 
                $(tileId).addClass("in-focus");
            else
                $(tileId).addClass("opposition-in-focus");
        }
        else if (message.Type === 'TileBlur') {
            console.log(message.Content);
            var tileFocusEvent = JSON.parse(message.Content);
            var tileId = '#' + tileFocusEvent.XPos + "-" + tileFocusEvent.YPos;

            $(tileId).removeClass("in-focus");
            $(tileId).removeClass("opposition-in-focus");
        }
    };
}

function createGameContainer(){
    var tileHtml = '<div class="col-xs-12 tile" id="TILEIDGOESHERE">' +
                        '<input id="TILEINPUTIDGOESHERE" class="tile-input" onBlur="inputBlur(PARAMETERSGOHERE)"  onFocus="inputOnFocus(PARAMETERSGOHERE)"/>' +
                    '</div>';

    for (var i = 0; i < 9; i++) {
        var rowHtml = '<div class="row">';
        for (var j = 0; j < 9; j++) {
            var specificTileHtml = tileHtml.replace('PARAMETERSGOHERE', j + ', ' + i)
                                        .replace('PARAMETERSGOHERE', j + ', ' + i)
                                        .replace('TILEIDGOESHERE', j + "-" + i)
                                        .replace('TILEINPUTIDGOESHERE', "TileInputId-" + j + "-" + i);

            rowHtml += specificTileHtml;
        }
        rowHtml += '</div>'
        $('#gameContainer').append(rowHtml);
    }

    $('#gameContainer').show();
}

function inputOnFocus(xPos, yPos) {
    var message = {};
    message.XPos = xPos;
    message.YPos = yPos;
    message.UserId = userId;
    sendMessage('TileFocus', message);
}

function inputBlur(xPos, yPos) {
    var message = {};
    message.XPos = xPos;
    message.YPos = yPos;
    message.UserId = userId;

    var tileId = '#TileInputId-' + xPos + "-" + yPos;
    message.Value = $(tileId).val();

    sendMessage('TileBlur', message);
}

function sendMessage(type, content) {
    var message = {};
    message.Type = type;
    message.GameId = gameId;
    message.Content = JSON.stringify(content);
    socket.send(JSON.stringify(message));
}