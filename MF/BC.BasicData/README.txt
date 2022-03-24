v1.0.0 基础版
v1.0.1 发布版
v1.0.2-rc3 CustomeEvetnArgs补充字段
v1.0.2-rc11 提高日志性能
      本次升级相应修改业务如下:
      new ApplicationLog();
      List<string> OpNames= new List<string>();
      OpNames.Add("OP010");
      OpNames.Add("OP020");
      ApplicationLog.IntLogThread(OpNames);
v1.0.3-rc5 发布版
v1.0.3-rc7 优化日志性能
v1.0.3 稳定版