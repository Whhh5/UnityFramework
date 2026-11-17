mergeInto(LibraryManager.library, {
    // 创建透明原生授权按钮
    CreatePhoneAuthButton: function(xPtr, yPtr, widthPtr, heightPtr, gameObjPtr, successMethodPtr, failMethodPtr) {
        var x = parseFloat(Pointer_stringify(xPtr));
        var y = parseFloat(Pointer_stringify(yPtr));
        var width = parseFloat(Pointer_stringify(widthPtr));
        var height = parseFloat(Pointer_stringify(heightPtr));
        var gameObj = Pointer_stringify(gameObjPtr);
        var successFunc = Pointer_stringify(successMethodPtr);
        var failFunc = Pointer_stringify(failMethodPtr);
        
        // 创建原生按钮
        var btn = document.createElement('button');
        btn.id = 'phoneAuthBtn';
        btn.style.position = 'absolute';
        btn.style.left = x + 'px';
        // Unity屏幕坐标原点在左下，HTML在左上，用画布高度换算Y轴
        var canvasHeight = document.getElementById('GameCanvas').height;
        btn.style.bottom = y + 'px';
        btn.style.width = width + 'px';
        btn.style.height = height + 'px';
        btn.style.opacity = '0'; // 完全透明
        btn.style.background = 'transparent';
        btn.style.border = 'none';
        btn.style.padding = '0';
        btn.style.zIndex = '1000';
        btn.style.touchAction = 'none';
        
        // 原生touchend事件中直接调用授权
        btn.addEventListener('touchend', function(e) {
            wx.getPhoneNumber({
                success: function(res) {
                    wx.getUnityContext().sendMessage(gameObj, successFunc, JSON.stringify(res));
                },
                fail: function(err) {
                    wx.getUnityContext().sendMessage(gameObj, failFunc, JSON.stringify(err));
                }
            });
            e.preventDefault();
        });
        
        document.body.appendChild(btn);
    },
    
    // 控制按钮显示隐藏
    SetPhoneAuthButtonVisible: function(visible) {
        var btn = document.getElementById('phoneAuthBtn');
        if (btn) {
            btn.style.display = visible ? 'block' : 'none';
        }
    }
});
