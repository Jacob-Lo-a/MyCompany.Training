### 第 1 天：環境建置與 .NET CLI 基礎
1. 建立空白專案
2. 在專案中新增 `類別庫(Training.Core)` 和 `Web API(Training.API)`
3. 在 Training.Core 裡寫一個 `SystemInfo` 類別，寫一個方法`GetOSAndDateTime()`回傳目前的電腦作業系統名稱與時間
4. 在 Training.API 的 Program.cs 呼叫這個方法
5. 使用 `dotnet watch` 打開 Swagger 網頁

[參考文件](https://reurl.cc/5b7Anq)

### 第 2 天：C# 12 現代化語法精進
1. 使用主要建構子方法建立 `Order類別`
2. 宣告兩個`int`陣列，使用集合運算式組成一個陣列
3. 寫一個switch方法`Discount`，傳入"VIP"回傳0.8、"Normal"回傳、其他字串拋出 `ArgumentException` 錯誤 
