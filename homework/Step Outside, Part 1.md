# UVP Homework 20190314

1. Please choose a control fom XAML Control Gallery that can contribute to your app.

   ListView

2. Explain why it would contribute to your app.

   Use ListView for showing file lists.

3. Choose a client dev platform, e.g., Android, Qt, iOS, that you are not familiar with.

   Android

4. Figure out if your chosen platform supports that control. If so, demonstrate how to use it.

   Android supports ListView.

   

## 才明洋

1. NavigationView
2. NavigationView将页面分为两部分：左侧的汉堡菜单和右侧的内容部分。通过左侧的汉堡菜单可以在多个页面中进行切换，在文件管理器中，可以将虚拟目录放在菜单中，通过点击在不同页面中切换。
3. html可以通过`css`和`js`脚本实现这个控件，实现方法如下：

~~~html
<!DOCTYPE html>
<html>
<head>
<meta name="viewport" content="width=device-width, initial-scale=1">
<style>
body {
  font-family: "Lato", sans-serif;
}

.sidebar {
  height: 100%;
  width: 0;
  position: fixed;
  z-index: 1;
  top: 0;
  left: 0;
  background-color: #111;
  overflow-x: hidden;
  transition: 0.5s;
  padding-top: 60px;
}

.sidebar a {
  padding: 8px 8px 8px 32px;
  text-decoration: none;
  font-size: 25px;
  color: #818181;
  display: block;
  transition: 0.3s;
}

.sidebar a:hover {
  color: #f1f1f1;
}

.sidebar .closebtn {
  position: absolute;
  top: 0;
  right: 25px;
  font-size: 36px;
  margin-left: 50px;
}

.openbtn {
  font-size: 20px;
  cursor: pointer;
  background-color: #111;
  color: white;
  padding: 10px 15px;
  border: none;
}

.openbtn:hover {
  background-color: #444;
}

#main {
  transition: margin-left .5s;
  padding: 16px;
}

/* On smaller screens, where height is less than 450px, change the style of the sidenav (less padding and a smaller font size) */
@media screen and (max-height: 450px) {
  .sidebar {padding-top: 15px;}
  .sidebar a {font-size: 18px;}
}
</style>
</head>
<body>

<div id="mySidebar" class="sidebar">
  <a href="javascript:void(0)" class="closebtn" onclick="closeNav()">×</a>
  <a href="#">About</a>
  <a href="#">Services</a>
  <a href="#">Clients</a>
  <a href="#">Contact</a>
</div>

<div id="main">
  <button class="openbtn" onclick="openNav()">☰ Toggle Sidebar</button>  
  <h2>Collapsed Sidebar</h2>
  <p>Click on the hamburger menu/bar icon to open the sidebar, and push this content to the right.</p>
</div>

<script>
function openNav() {
  document.getElementById("mySidebar").style.width = "250px";
  document.getElementById("main").style.marginLeft = "250px";
}

function closeNav() {
  document.getElementById("mySidebar").style.width = "0";
  document.getElementById("main").style.marginLeft= "0";
}
</script>
   
</body>
</html> 
~~~




## 王明建

1. Declare a ListView in xml file.

   ```xml
   <ListView android:id="@+id/list_view" android:layout_width="match_parent" android:layout_height="match_parent" />
   ```

2. Create a adapter class for display data. When it is trying to show a list, Android will call the following function for every item, the function should put the <i>position-th</i> item's content into <i>convertView</i>.

   ```java
   private class MyAdapter extends BaseAdapter { 
       @Override
       public View getView(int position, View convertView, ViewGroup container) {
           if (convertView == null) { 
               convertView = getLayoutInflater().inflate(
                   R.layout.list_item, container, false); 
           } 
           ((TextView) convertView.findViewById(android.R.id.text1)) .setText(getItem(position)); 
           return convertView; 
       } 
   }
   ```

3. Combine ListView and Adapter together in initialize.

   ``` java
   (ListView)listView = findViewById(R.id.list_view);
   listView.setAdapter(new MyAdapter);
   ```

## 张晨

1. Please choose a control from XAML Control Gallery that can contribute to your app.

   ​	ToolTip

2. Explain why it would contribute to your app.

   ​	工具栏是必须的

3. Choose a client dev platform, e.g., Android, Qt, iOS, that you are not familiar with.

   ​	微信小程序

4. Figure out if your chosen platform supports that control. If so, demonstrate how to use
   it.

   ​	支持tabBar

5. 具体实现

   ​	在app.json中添加tabBar并且简单配置

   ![](.\stepout-part1-pic1.png)

6. 最后效果

   ![](.\stepout-part1-pic2.jpg)

