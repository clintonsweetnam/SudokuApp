
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

var gameId;
var userId;
var focusTile;

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

    $('#header-logo').attr('data-original-title', data.gameId)
          .tooltip('fixTitle');

    $('#initializationButtons').hide();

    createGameContainer();

    for (var xPos = 0; xPos < 9; xPos++) {
        for (var yPos = 0; yPos < 9; yPos++) {
            var id = '#TileInputId-' + xPos + "-" + yPos;
            var value = data.board[xPos][yPos];
            if (value != 0)
                $(id).val(value);
        }
    }

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
            var tileFocusEvent = JSON.parse(message.Content);
            var tileId = '#TileInputId-' + tileFocusEvent.XPos + "-" + tileFocusEvent.YPos;

            var tileValue = $(tileId).val();

            if (tileValue === '')
            {
                if (tileFocusEvent.UserId === userId) 
                    $(tileId).addClass("in-focus");
                else
                    $(tileId).addClass("opposition-in-focus");
            }
        }
        else if (message.Type === 'TileBlur') {
            var tileFocusEvent = JSON.parse(message.Content);
            var tileId = '#TileInputId-' + tileFocusEvent.XPos + "-" + tileFocusEvent.YPos;

            $(tileId).removeClass("in-focus");
            $(tileId).removeClass("opposition-in-focus");
        }
    };
}

function makeGuess(guess)
{
    var message = {};
    message.XPos = focusTile[0];
    message.YPos = focusTile[1];
    message.UserId = userId;
    message.Guess = guess;
    sendMessage('MakeGuess', message);
}

function createGameContainer(){
    var tileHtml = '<div class="col-xs-12 tile EXTRACSSCLASSES" id="TILEIDGOESHERE">' +
                        '<input id="TILEINPUTIDGOESHERE" readonly="true" class="tile-input" onBlur="inputBlur(PARAMETERSGOHERE)"  onFocus="inputOnFocus(PARAMETERSGOHERE)"/>' +
                    '</div>';

    for (var i = 0; i < 9; i++) {
        var rowHtml = '<div class="row">';
        for (var j = 0; j < 9; j++) {
            var cssClasses = '';

            if (i === 0 || i === 3 || i === 6)
                cssClasses += ' heavy-top-border';
            if (i === 8)
                cssClasses += ' heavy-bottom-border';
            if (j === 0 | j === 3 || j === 6)
                cssClasses += ' heavy-left-border'
            if (j === 8)
                cssClasses += ' heavy-right-border'

            var specificTileHtml = tileHtml.replace('PARAMETERSGOHERE', j + ', ' + i)
                                        .replace('PARAMETERSGOHERE', j + ', ' + i)
                                        .replace('TILEIDGOESHERE', j + "-" + i)
                                        .replace('TILEINPUTIDGOESHERE', "TileInputId-" + j + "-" + i)
                                        .replace('EXTRACSSCLASSES', cssClasses);

            rowHtml += specificTileHtml;
        }
        rowHtml += '</div>'

        $('#gameContainer').append(rowHtml);
    }

    var windowWidth = window.innerWidth - 22;
    var tileSize = windowWidth / 9;

    var elements = document.getElementsByClassName("tile");
    for (var i = 0; i < elements.length; i++) {
        elements[i].style.width = (tileSize + "px");
        elements[i].style.height = (tileSize + "px");
    }

    var elements = document.getElementsByClassName("tile-input");
    for (var i = 0; i < elements.length; i++) {
        elements[i].style.width = (tileSize - 1 + "px");
        elements[i].style.height = (tileSize - 1 + "px");
    }

    $('#gameContainer').show();
}

function inputOnFocus(xPos, yPos) {
    var message = {};
    focusTile = [xPos, yPos]
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