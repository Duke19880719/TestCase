<script setup>
  import { ref } from 'vue'
  import AssetTable from '@/components/AssetTable.vue'

  const activeTab = ref(0)
  const year = ref(2026)
  const tabs = ['資產盤點與評鑑', '資產項目維護', '近期資產清冊']
</script>

<template>

  <!-- Navbar 滿版 -->
  <header class="navbar">
    <div class="nav-inner">

      <div class="logo"><img src="@/assets/logo_system.jpg"/></div>

      <nav class="menu">
        <a class="home_page"><img src="@/assets/home.png"/></a>
        <a  href="#">數位儀錶板</a>
        <a  href="#">資產清冊</a>
        <a  href="#">軟體管理</a>
        <a  href="#">線上服務</a>
        <a  href="#">統計管理</a>
        <a  href="#">系統管理</a>
        <a><img width="100" height="100" src="https://img.icons8.com/ios-filled/100/bulleted-list.png" alt="bulleted-list" /></a>
      </nav>

      <button class="login-btn">登入</button>

    </div>
  </header>


  <!-- 主內容 -->
  <main class="page">

    <div class="container0">

      <!-- Title -->
      <h1 class="page-title">資訊資產清冊暨風險評鑑</h1>

      <!-- Tabs -->
      <div class="tabs">
        <button v-for="(tab, index) in tabs"
                :key="index"
                class="tab"
                :class="{ 'active': activeTab === index }"
                @click="activeTab = index">
          {{ tab }}
        </button>
      </div>

      <!-- Action Bar -->
      <div class="action-bar">

        <div class="year-select">
          資料填報年度：
          <select v-model="year">
            <option>2026</option>
            <option>2025</option>
          </select>
        </div>

        <div class="actions">
          <button class="btn gray">風險評鑑說明</button>
          <button class="btn gray">去年資料帶入</button>
          <button class="btn red">關檔</button>
        </div>

      </div>

      <!-- table -->
      <div class="table-wrapper">
        <div v-if="activeTab === 0"> <AssetTable v-bind:year="Number(year)" /> </div>
        <div v-if="activeTab === 1">這是 資產項目維護 的內容</div>
        <div v-if="activeTab === 2">這是 近期資產清冊 的內容</div>
      </div>

    </div>

  </main>

</template>

<style scoped>
  .logo {
    position: relative;
    font-weight: 600;
    font-size: 18px;
    display: flex;
    height: 100%;
    align-items: center;
    gap: 8px;
  
  }
 


  .navbar {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    background: #c8102e;
    z-index: 9999;
    box-shadow: 0 2px 6px rgb(0 0 0 / 0.15);
    
  }

  .nav-inner {
    width: 100%;
    margin-top: 0;
    margin-bottom: 0;
    margin-left:0;
    height: 56px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    color: white;
  }


 


  .menu {
    display: flex;
    margin-left: 30px;
    flex: 1;
    height: 100%;
  }
    /* 針對 menu 內的 a 標籤做微調，確保圖片與文字對齊 */
    .menu a {
      display: flex;
      align-items: center;
      justify-content: center;
      color: white;
      text-decoration: none;
      padding: 0 15px;
      font-size: 15px;
      height: 100%; /* 確保高度撐滿導覽列 */
      transition: background 0.2s;
    }

      /* 針對 menu 內的圖片 (home.png) */
      .menu a img {
        position:relative;
        width: 100%; /* 根據截圖，icon 大小通常約 20px 左右 */
        height: 60%;
        object-fit: contain;
        filter: invert(1); /* 關鍵：將黑色圖片反白變白色 */
        display: block;
 
      }
      .home_page::after {
        content: "";
        position: absolute;
/*        left: 0;*/
        bottom: 0px;
        width: 50px;
        height: 4px;
        background: white;
        z-index: 20;
      }

      /* 如果是第一個 a (通常是首頁 icon)，可以縮減 padding 讓它更緊湊 */
      .menu a:first-child {
        padding: 0 12px;
      }

      /* 滑過與啟動樣式保持不變 */
      .menu a:hover {
        background: rgba(255, 255, 255, 0.1);
      }


      .menu a:active {
        background: #a10b24; /* 截圖中 active 是深色塊 */
        font-weight: 600;
        /* 加上白色邊框 */
        outline: 2px solid white; /* 使用 outline 不會影響元素佔用的寬高 */
        outline-offset: -1px; /* 讓邊框向內縮，避免貼到邊緣 */
      }



  .login-btn {
    position:absolute;
    background: white;
    color: #333;
    border: none;
    padding-top: 4px;
    padding-bottom: 4px;
    right: 40px;
    border-radius: 4px;
    font-size: 13px;
    cursor: pointer;
    font-weight: 500;
  }

  /* ====== Main Area ====== */

  main {
    padding-top: 56px; /* 預留 navbar 高度 */
    width: 100%;
    background: #f2f2f2;
    min-height: calc(100vh - 56px);
  }

    main.page {
      flex: 1; /* 自動撐開剩餘高度 */
      width: 100%;
      background: #f2f2f2;
      display: flex;
      flex-direction: column;
    }

  .container0 {
    width: 100%;
    flex: 1;
    margin: 0; /* 移除 margin: 0 auto */
    padding: 20px; /* 如果要完全貼邊可以設為 0，但通常會留 15-20px 內距 */
    background: white;
    border-radius: 0; /* 滿版時通常不需要圓角 */
    box-shadow: none; /* 滿版時陰影較不明顯，可拿掉 */
    box-sizing: border-box; /* 確保 padding 不會撐破寬度 */
  }

  .page-title {
    font-size: 18px;
    color: black;
    margin: 10px 0 20px 0;
    display: flex;
    align-items: center;
    gap: 8px;
  }

    .page-title::before {
      content: "";
      display: inline-block;
      width: 25px;
      height: 25px;
      background: url("@/assets/pen_logo.png");
      background-size: contain;
      background-repeat: no-repeat;
      filter: invert(1); /* 黑色 → 白色 */
    }
    

  /* ====== Tabs ====== */
  .tabs {
    width: 100%;
    display: flex;
    gap: 2px;
    margin-bottom: 20px;
    border-bottom: 2px solid gray;
    
  }

  .tab {
    padding: 8px 16px;
    border: none;
    background: white;
    font-size: 14px;
    cursor: pointer;
    /*    color: #666;*/
    /*    border: 1px solid #ddd;*/
    border-bottom: none;
    border-radius: 4px 4px 0 0;
  }

    .tab.active {
      background: white;
/*      color: #000;*/
      font-weight: 600;
      position: relative;
    }

      /* 截圖中的 active tab 紅色細線在頂部 */
      .tab.active::after {
        content: '';
        position: absolute;
        top: 100%;
        left: 0;
        right: 0;
        height: 3px;
        background: #c8102e;
      }

  /* ====== Action Bar ====== */
  .action-bar {
  
    display: flex;
    background: #f8f8f8; /* 加上淺灰底色區分 */
    padding: 10px;
    border: 1px solid #eee;
    margin-bottom: 15px;
  }

  .actions {
    position:relative;
    display: flex;
    gap: 5px;
    margin-left:5px;
  }

  .btn {
    padding: 5px 12px;
    border-radius: 3px;
    font-size: 13px;
    display: flex;
    align-items: center;
    gap: 4px;
  }

    /* 模擬截圖中按鈕前的圖示 */
    .btn.gray::before {
      content: 'ℹ️';
      font-size: 12px;
    }

    .btn.red::before {
      content: '🔒';
      font-size: 12px;
    }

  /* ====== Table ====== */
  .table-wrapper {
    width: 100%;
    overflow-x: auto;
/*    border: 1px solid #ccc;*/
  }

  table {
    width: 100%;
    border-collapse: collapse;
    /* 移除 min-width 或者設為 100% 讓它隨容器縮放 */
  }

  thead th {
    background: #f0f0f0; /* 標題淺灰色面 */
    color: #333;
    font-weight: 500;
    border: 1px solid #ccc;
    padding: 8px;
  }

  tbody td {
    border: 1px solid #ddd;
    padding: 8px;
    color: #444;
  }

  /* 特定欄位對齊 */
  td:nth-child(1), /* 序號 */
  td:nth-child(4), /* C */
  td:nth-child(5), /* I */
  td:nth-child(6) /* A */ {
    text-align: center;
    font-family: monospace; /* 數值建議用等寬字 */
  }

  /* 風險值欄位背景色 (模擬截圖效果) */
  td:nth-child(10) {
    background: #fff9c4; /* 淡淡的黃色提醒 */
    font-weight: bold;
  }

</style>
