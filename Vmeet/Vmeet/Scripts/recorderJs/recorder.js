/*License (MIT)

Copyright © 2013 Matt Diamond

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

(function (window) {
    var bloob = new Blob([document.querySelector('#worker1').textContent], { type: "text/javascript" });

    var WORKER_PATH = window.URL.createObjectURL(bloob);

    function downsampleBuffer(buffer, originalRate, rate) {
        if (rate == originalRate) {
            return buffer;
        }
        if (rate > originalRate) {
            throw "downsampling rate show be smaller than original sample rate";
        }
        var sampleRateRatio = originalRate / rate;
        var newLength = Math.round(buffer.length / sampleRateRatio);
        var result = new Float32Array(newLength);
        var offsetResult = 0;
        var offsetBuffer = 0;
        while (offsetResult < result.length) {
            var nextOffsetBuffer = Math.round((offsetResult + 1) * sampleRateRatio);
            var accum = 0, count = 0;
            for (var i = offsetBuffer; i < nextOffsetBuffer && i < buffer.length; i++) {
                accum += buffer[i];
                count++;
            }
            result[offsetResult] = accum / count;
            offsetResult++;
            offsetBuffer = nextOffsetBuffer;
        }
        return result;
    }

    var Recorder = function (source, cfg) {
        var config = cfg || {};
        var bufferLen = config.bufferLen || 2048;
        var channelId = -1;
        var ToplantiId = Number( document.getElementById("ToplantiId").getAttribute("value"));
        this.context = source.context;
        if (!this.context.createScriptProcessor) {
            this.node = this.context.createJavaScriptNode(bufferLen, 2, 2);
        } else {
            this.node = this.context.createScriptProcessor(bufferLen, 2, 2);
        }
        var samplerate = this.context.sampleRate;

        var worker = new Worker(config.workerPath || WORKER_PATH);
        worker.postMessage({
            command: 'init',
            config: {
                sampleRate: this.context.sampleRate
            }
        });
        var recording = false,
          currCallback;

        this.node.onaudioprocess = function (e) {
            if (!recording) return;
            //worker.postMessage({
            //    command: 'record',
            //    buffer: [
            //      e.inputBuffer.getChannelData(0),
            //      e.inputBuffer.getChannelData(1)
            //    ]
            //});
            if (channelId == -1)
                return;
            var arrayBuffer = e.inputBuffer.getChannelData(0);
            arrayBuffer = downsampleBuffer(arrayBuffer, samplerate, 9600);
            var array = [];
            for (var i = 0; i < arrayBuffer.length; i++) array[i] = arrayBuffer[i];
            $.connection.chatHub.server.speak(channelId, array);
        }

        this.configure = function (cfg) {
            for (var prop in cfg) {
                if (cfg.hasOwnProperty(prop)) {
                    config[prop] = cfg[prop];
                }
            }
        }

        this.record = function () {
            $.connection.chatHub.server.startSpeaking(ToplantiId);
            setTimeout(function () {
                channelId = $('#ChannelId').val();
            }, 1000);
            recording = true;
        }

        this.stop = function () {
            $.connection.chatHub.server.stopSpeaking(ToplantiId,channelId);
            channelId = -1;
            recording = false;
        }

        this.clear = function () {
            worker.postMessage({ command: 'clear' });
        }

        this.getBuffers = function (cb) {
            currCallback = cb || config.callback;
            worker.postMessage({ command: 'getBuffers' });
        }

        this.exportWAV = function (cb, type) {
            currCallback = cb || config.callback;
            type = type || config.type || 'audio/wav';
            if (!currCallback) {
                throw new Error('Callback not set');
            }
            worker.postMessage({
                command: 'exportWAV',
                type: type
            });
        }

        this.exportMonoWAV = function (cb, type) {
            currCallback = cb || config.callback;
            type = type || config.type || 'audio/wav';
            if (!currCallback) throw new Error('Callback not set');
            worker.postMessage({
                command: 'exportMonoWAV',
                type: type
            });
        }

        worker.onmessage = function (e) {
            var blob = e.data;
            currCallback(blob);

        };

        source.connect(this.node);
        this.node.connect(this.context.destination);   // if the script node is not connected to an output the "onaudioprocess" event is not triggered in chrome.
    };

    Recorder.setupDownload = function (blob, filename) {
        var url = (window.URL || window.webkitURL).createObjectURL(blob);
        var link = document.getElementById("save");
        link.href = url;
        link.download = filename || 'output.wav';
    };

    window.Recorder = Recorder;

})(window);
