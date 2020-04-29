# TaiwuTools

<p align="center">
  <span>English</span> |
  <a href="README.cn.md">中文</a>
</p>

---
+ [Introduction](#Introduction)
+ [ManagedGameObject](#ManagedGameObject)
	+ [Unity UI Kits](#Unity-UI-Kit)
	+ [Taiwu UI Kits](#Taiwu-UI-Kit)

## Introduction

- [The Scroll Of Taiwu](https://store.steampowered.com/app/838350/_The_Scroll_Of_Taiwu/) is a independent game developed by [Conch Ship](https://www.conchship.com.cn/).
- This project isn't official mod, and source code and the released program do not contain any game asset files.**Do not distribute the game resource files or source code generated by the kits, otherwise the user must bear the legal liability.**

## ManagedGameObject

### Unity UI Kit
+ Core

  + BoxElementGameObject

    Use LayoutElement.  
    Can auto resize itself by parm.  

  + BoxSizeFitterGameObject

    Use ContentSizeFitter.  
    Can resize parent to adapt children.  

  + BoxGroupGameObject

    Use LayoutGroup.  
    It support Horizontal and Vertical.  
    Can auto sort and resize children.  

  + BoxGirdGameObject(Base on BoxSizeFitterGameObject)

    Use GridLayoutGroup.  
    But it can automatically set children's width to adapt parent.  

  + BoxModelGameObject(Base on BoxElementGameObject,BoxGroupGameObject)

    Have both BoxElementGameObject on and BoxGroupGameObject function.  

  + BoxAutoSizeModelGameObject(Base on BoxSizeFitterGameObject,BoxGroupGameObject)

    Have both BoxSizeFitterGameObject and BoxGroupGameObject function.  

+ Non-core

  They are only base class.  
  (Mean that a large part of them aren't used easily.)  

  + Label

    Text of Unity.  

    + Need to complete description.  

  + Slider

    Slider of Unity.  

    + Need to complete description.

  + Base Toggle Button

    DON NOT USE THIS!  
    It is base class of Toggle and Button.  

    + Toggle

      It is base class of Toggle.  
      (But it can work.)  

    + Toggle Group

      It is Container,but it can also set toggleGroup of toggle that in children.

    + Button

      It is base class of Button.  
      (But it can work.)  

  + Container

    It is BoxModelGameObject but it has background.
      + Need to complete description.

    + Container.Canvas

      Add Canvas UGUI.
        + Need to complete description.

    + Container.Scroll

      List View.
        + Need to complete description.

    + Container.GridContainer

      Container but use BoxGrid
        + Need to complete description.

    + Container.FitterContainer

      Container but use BoxAutoSizeModelGameObject
        + Need to complete description.

  + Block

    It is BoxElementGameObject but it has background.
      + Need to complete description.

### Taiwu UI Kit
+ Need to complete description.