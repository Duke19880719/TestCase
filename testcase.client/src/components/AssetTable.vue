<template>
  <div class="table-container">

    <div class="table-header">
      <button class="add-btn" @click="addRecord">⊕ 新增記錄</button>
    </div>

    <table class="asset-table">
      <thead>
        <tr>
          <th>序號</th>
          <th>類別</th>
          <th>資產項目</th>
          <th>資產價值(C)</th>
          <th>資產價值(I)</th>
          <th>資產價值(A)</th>
          <th>資產價值</th>
          <th>資產等級</th>
          <th>威脅-1</th>
          <th>脆弱點-1</th>
          <th>風險值-1</th>
          <th>風險擁有者</th>
          <th>編輯/刪除</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="(item,index) in assets" :key="index">
          <!-- id -->
          <td>{{ item.Id || "-" }}</td>

          <!-- type -->
          <td>
            <span v-if="!(is_create && edit_index === index) && !(edit_index === index && !is_create)">{{ item.Type }}</span>
            <input v-else-if="is_create && edit_index === index" v-model="edit_item.Type">
            <span v-else>{{ item.Type }}</span>
          </td>

          <!-- name -->
          <td>
            <span v-if="!(is_create && edit_index === index) && !(edit_index === index && !is_create)">{{ item.Name }}</span>
            <input v-else-if="is_create && edit_index === index" v-model="edit_item.Name">
            <span v-else>{{ item.Name }}</span>
          </td>

          <!-- c/i/a -->
          <td>
            <input v-if="edit_index === index" type="number" v-model="edit_item.C">
            <span v-else>{{ item.C }}</span>
          </td>
          <td>
            <input v-if="edit_index === index" type="number" v-model="edit_item.I">
            <span v-else>{{ item.I }}</span>
          </td>
          <td>
            <input v-if="edit_index === index" type="number" v-model="edit_item.A">
            <span v-else>{{ item.A }}</span>
          </td>

          <!-- value / level / risk → 新增才可輸入 -->
          <td>
            <input v-if="is_create && edit_index === index" type="number" v-model="edit_item.Value">
            <span v-else>{{ item.Value }}</span>
          </td>
          <td>
            <input v-if="is_create && edit_index === index" type="number" v-model="edit_item.Level">
            <span v-else>{{ item.Level }}</span>
          </td>
          <td>
            <input v-if="is_create && edit_index === index" type="number" v-model="edit_item.Risk">
            <span v-else>{{ item.Risk }}</span>
          </td>

          <!-- threat / weakness -->
          <td>
            <input v-if="edit_index === index" v-model="edit_item.Threat">
            <span v-else>{{ item.Threat }}</span>
          </td>
          <td>
            <input v-if="edit_index === index" v-model="edit_item.Weakness">
            <span v-else>{{ item.Weakness }}</span>
          </td>

          <!-- owner -->
          <td>
            <input v-if="is_create && edit_index === index" v-model="edit_item.Owner">
            <span v-else>{{ item.Owner }}</span>
          </td>

          <!-- 操作 -->
          <td>
            <template v-if="is_create && edit_index === index">
              <span @click="createRecord(index)">新增</span> /
              <span @click="cancelEdit">取消</span>
            </template>
            <template v-else-if="edit_index === index && !is_create">
              <span @click="saveRecord(index)">更新</span> /
              <span @click="cancelEdit">取消</span>
            </template>
            <template v-else>
              <span class="edit" @click="editRecord(index)">✏️</span>
              <span class="delete" @click="deleteRecord(index)">🗑️</span>
            </template>
          </td>
        </tr>
      </tbody>

    </table>

  </div>
</template>

<script setup>

  import { ref, onMounted, watchEffect, watch } from "vue"
  import axios from "axios"

  const props = defineProps({
    year: Number
  })
  // 資料
  const assets = ref([])

// 取得 API 資料
  const fetchAssets = async () => {
    const res = await axios.get(
      `https://localhost:7004/api/GetAssets?year=${props.year}`
    )
    console.log(res.data) 
    assets.value = res.data
  }

  // 新增
const is_create = ref(false)
const addRecord = () => {

  edit_item.value = {
    Id: "",
    Type: "",
    Name: "",
    C: 0,
    I: 0,
    A: 0,
    Value: 0,
    Level: 0,
    Threat: "",
    Weakness: "",
    Risk: 0,
    Owner: "",
    Year: props.year
  }

  // 在 table 最前面加入 edit_item
  assets.value.unshift(edit_item.value)

  // 設定 edit_index 為第一筆
  edit_index.value = 0
  is_create.value = true
}

// 儲存新增 row
  const createRecord = async (index) => {
    try {
      const payload = {
        Id: "",
        Type: String(edit_item.value.Type),
        Name: String(edit_item.value.Name),
        C: Number(edit_item.value.C),
        I: Number(edit_item.value.I),
        A: Number(edit_item.value.A),
        Value: Number(edit_item.value.Value),
        Level: Number(edit_item.value.Level),
        Threat: String(edit_item.value.Threat),
        Weakness: String(edit_item.value.Weakness),
        Risk: Number(edit_item.value.Risk),
        Owner: String(edit_item.value.Owner),
        Year: Number(edit_item.value.Year)
      }

      const res = await axios.post(
        "https://localhost:7004/api/CreateAsset",
        payload
      )

      // 將原本 unshift 進去的空殼資料替換成後端帶有 ID 的正式資料
      assets.value[0] = res.data

      // 重置狀態
      edit_index.value = null
      is_create.value = false
      edit_item.value = {}

      alert("新增成功")

    } catch (err) {
      console.error(err)
      alert("新增失敗")
    }
  }

  // 編輯
const edit_index = ref(null)
const edit_item = ref({})

const editRecord = (index) => {
  edit_index.value = index;
  edit_item.value = { ...assets.value[index] }
}

const saveRecord = async (index) => {
  
  try {
   
    const payload = {
      ...edit_item.value
    }

    await axios.put(
      "https://localhost:7004/api/UpdateAsset",
      payload
    )

   // 更新本地資料
   assets.value[index] = { ...edit_item.value }

   edit_index.value = null

  } catch (err) {

    console.error(err)
    alert("更新失敗")

  }
}

const cancelEdit = () => {
  if (is_create.value) {
    // 移除 table 最前面這筆未送出的資料
    assets.value.shift()
    is_create.value = false
  }
  edit_index.value = null
}

// 刪除
const deleteRecord = async (index) => {
  const asset = assets.value[index]

  if (!asset || !asset.Id) {
    alert("找不到要刪除的資產")
    return
  }

  if (!confirm(`確定要刪除資產 "${asset.Name}" 嗎？`)) return

  try {
    // 用正確的 Id 放在 URL
    await axios.delete(`https://localhost:7004/api/DeleteAsset/${asset.Id}`)

    // 刪除成功後更新前端 table
    assets.value.splice(index, 1)

  } catch (err) {
    console.error(err)
    alert("刪除失敗")
  }
}

// component mounted
onMounted(() => {
      fetchAssets()
})

watch(() => props.year, () => {
  fetchAssets()
})

</script>


<style scoped>


  .table-container {
    width: auto;
/*    margin: 20px auto;*/
    background: white;
/*    padding: 10px;*/
    border-radius: 6px;
  }

  .table-header {
    text-align: right;
    margin-bottom: 10px;
    background-color: #eee;
  }

  .add-btn {
    background: #eee;
    border: none;
    padding: 6px 12px;
    cursor: pointer;
    border-radius: 4px;
  }

    .add-btn:hover {
      background: #ddd;
    }

  .asset-table {
    width: 100%;
    border-collapse: separate; 
    border-spacing: 2.5px 5px; /* 水平 10px，垂直 5px */
    font-size: 14px;
  }

    .asset-table thead {
      background: #d9d9d9;
    }

    .asset-table th,
    .asset-table td {
      padding: 10px 8px;
      text-align: center;
      border-right: 1px solid #eee;
    }

      .asset-table th:last-child,
      .asset-table td:last-child {
        border-right: none;
      }

    .asset-table tbody tr {
      border-bottom: 1px solid #eee;
    }

      .asset-table tbody tr:hover {
        background: #fafafa;
      }

  .action span {
    cursor: pointer;
    margin: 0 5px;
  }

  .alert {
    display: inline-block;
    width: 16px;
    height: 16px;
    background: red;
    color: white;
    font-size: 12px;
    line-height: 16px;
    text-align: center;
    border-radius: 50%;
    margin-left: 5px;
  }
</style>
