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
3. 寫一個switch方法`Discount`，傳入"VIP"回傳0.8、"Normal"回傳0.9、其他字串拋出 `ArgumentException` 錯誤 

[參考文件](https://reurl.cc/qpz07y)

### 第 3 天：LINQ 資料操控與延遲執行
1. 建一個`List`，裡面塞 50 筆 `Student` 資料（姓名、分數、班級）
2. 練習實作 LINQ表達式 和 Lambda運算式
   1. 找出所有不及格的學生，依分數低到高排序
   2. 只找出所有學生的名字，組成一個新陣列
   3. 依照班級分組，算出A班和B班各自的平均分數

[參考文件LINQ](https://reurl.cc/ovzbzl)

[參考文件Lambda](https://reurl.cc/xWxbxN)

### 第 4 天：非同步 (Async/Await) 與死結預防
1. 寫一個方法 `DownloadDataAsync` ，裡面用 `Task.Delay(2000)` 假裝去網路下載資料耗時兩秒
2. 在 API 呼叫次方法兩次
   1. 加三次 `await`， API 總共花了 6 秒才回傳
   2. 用 `Task.WhenAll` 把三個任務包起來， API 只花了 2 秒就回傳
3. 加上 `CancellationToken (取消權杖)`，當網頁被關閉時，伺服器可以立刻停止下載

[參考資料](https://reurl.cc/8eloey)

### 第 5 天：單元測試 (xUnit) 與品質防護網
1. 針對第 3 天寫的「學生分數計算」，寫兩個測試案例
   1. 測試算出來的平均分數是否正確
   2. 如果傳入空的 `List`，會不會噴錯
2. 把原生的 `Assert.Equal(100, result)` 改成 `FluentAssertions` 的 `result.Should().Be(100)`
3. 寫一個 `IWeatherService` 介面，用 MOQ 創造一個假的 Service。設定呼叫 `GetTemp()` 時固定回傳25度，測試在25度時回不會回傳「天氣晴朗」

[參考資料 xUnit](https://learn.microsoft.com/zh-tw/dotnet/core/testing/unit-testing-csharp-with-xunit)

[參考資料 MOQ](https://ironpdf.com/zh-hant/blog/net-help/moq-csharp-guide/)

