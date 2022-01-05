# USTB Auto Login

北京科技大学校园网自动登录程序

## appsettings.json 样例
```json5
{
  "USTBLoginConfiguration": {
    "LoginAddress": "202.204.48.82",  // 校园网登录地址
                      // 可选其它包括 202.204.48.66, 222.199.222.14, login.ustb.edu.cn
    "CheckInternetConnectivityUrl": "https://www.baidu.com",  // 网络连通性检查所使用的地址
    "LoginInfos": [
      {
        "UserID": "********",   // 校园网登录用户名
        "Password": "********"  // 校园网登录密码
      },
      {
        "UserID": "********",   // 可以添加多组用户名与密码
        "Password": "********"
      }
    ],
    "CheckIntervalSecond": 15   // 网络连通性检查周期
  }
}
```

## 启动参数

* `-c appsettings.json 文件路径`：可选参数，参数不提供时，程序将从同级目录寻找此配置文件。

## 自动启动

### Windows

可配置 Windows 计划任务，或使用 [WinSW](https://github.com/winsw/winsw) 创建系统服务。

### Linux

可配置 systemctl 守护进程，需在 service 文件中显式指定 `appsettings.json` 文件的绝对路径。