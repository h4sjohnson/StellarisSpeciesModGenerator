# 《群星》种族立绘Mod生成器
## 生成Mod的功能
* 利用种族特质点来选择领袖和Pop的立绘组
* 根据立绘自动更名
## 步骤
### 1.准备图片资源
创建一个空的种族Mod，准备好图片资源，放入Mod文件夹（路径在稍后会填入设定表）。
### 2.填设定表
clone本项目。在项目根目录下，有如下几个设定表：
|  表名称   | 表作用  |
|  ----  | ----  |
|edict_localisation.csv | 重置立绘政策本地化设置|
|leader_portrait_list.csv | 领袖立绘ID/名称/路径|
|pop_portrait_list.csv | Pop小人立绘ID/路径|

该怎么填写，可以参考azurlane分支
### 3.运行
使用dotnet运行csproj， 文件会输出至/output文件夹内。最后将其中内容复制到Mod文件夹里。
```bat
#脚本示例
dotnet run SpeciesModGenerator.csproj
xcopy /e /y output ".../Documents/Paradox Interactive/Stellaris/mod/xxx Species Mod"
```