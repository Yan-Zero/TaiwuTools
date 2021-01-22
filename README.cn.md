# 赞助码
## 微信
![avatar](https://www.hualigs.cn/image/600aa12470350.jpg) 

# 太吾绘卷工具包

<p align="center">
    <a href="README.md">English</a> | <span>中文</span>
</p>

---
+ [简介](#简介)
+ [可用对象](#可用对象)
	+ [Unity UI 工具](#Unity-UI-工具)
	+ [Taiwu UI 工具](#Taiwu-UI-工具)

## 简介

- [太吾绘卷](https://store.steampowered.com/app/838350/_The_Scroll_Of_Taiwu/)是[螺舟工作室](https://www.conchship.com.cn/)所开发的独立游戏。
- 本项目为非官方 Mod，源码和发布的程序中不包含任何游戏资产文件。**请勿散播由工具集所生成的游戏资源文件或源码，由此造成的法律责任由使用者自己承担。**

## 可用对象

### Unity UI 工具
+ 核心组件 Core

  + 盒式元素对象 BoxElementGameObject

    使用元素布局（LayoutElement）。  
    可以根据参数自动调节大小。  
    
  + 盒式适配器对象 BoxSizeFitterGameObject

    使用内容适配器（ContentSizeFitter）。  
    可以调整亲元素使之适应子元素的大小。

  + 盒式组对象 BoxGroupGameObject

    使用组布局（LayoutGroup）。  
    支持水平或竖直布局。  
    可以自动排序并调整子元素大小。

  +  盒式网格对象 BoxGirdGameObject

    + 继承自盒式适配器对象

    使用网格组布局（GridLayoutGroup）。  
    会自动根据亲元素的大小调节子元素的宽度。

  + 盒式模型 BoxModelGameObject

    + 继承自盒式元素对象和盒式组对象
    
  + 盒式自适应模型 BoxAutoSizeModelGameObject

    + 继承自盒式适配器和盒式组对象

+ 非核心 Non-core

  这里只是一些基类，换言之，这些大都不怎么易用。
  
  + 标签 Label
  
    Unity 文本。
  
  + 滑条 Slider
  
    Unity 滑条。
  
  + 开关、按钮基础 Base Toggle Button
  
    开关和按钮的共同基类，不可直接使用！！！  
  
    + 开关 Toggle
  
      最基础的一种开关，可以直接使用。
  
    + 单选开关组 Toggle Group

      一种用来容纳多个开关的容器。  
      这个开关组的所有类型为开关的子元素都会和它绑定。
  
    + 按钮 Button
  
      最基础的一种按钮，可以直接使用。
  
  + 容器 Container
    
    一个拥有背景的盒式模型。
    
    + 画布容器 Container.Canvas
    
      添加画布 UGUI。
    
    + 滚动容器 Container.Scroll
    
      列表视图。
    
    + 网格容器 Container.GridContainer
    
      网格视图。
    
    + 适配容器 Container.FitterContainer
    
      使用了盒式自适应模型的容器。
    
  + 块 Block
  
    一个拥有背景的盒式元素。

### Taiwu UI 工具
暂无描述。
