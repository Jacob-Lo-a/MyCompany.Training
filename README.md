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

### 第 6 天：Middleware (中介軟體) 與請求生命週期
1. 寫一個「計算 API 執行時間」的 `Middleware` ，計算總花費時間並印出來
2. 寫一個「全域錯誤處理 (Global Exception Handling)」Middleware，只要拋出 Exception，用 middleware 攔截，並回傳包裝好的 JSON 錯誤格式

[參考資料](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/middleware?view=aspnetcore-10.0)

### 第 7 天：依賴注入 (DI) 的藝術
1. 實作一個 Guid 產生器，在 Minimal API 的建構子中，同時注入兩次 `IGuidGenerator`
2. 呼叫 API ，印出這兩個 Generator 產生的 Guid
   2.1 使用 `AddTransient` 時， Guid 不一樣
   2.2 改使用 `AddScoped` 時， Guid 是一樣的
  
[參考資料](https://learn.microsoft.com/zh-tw/dotnet/core/extensions/dependency-injection/usage)

### 第 8 天：設定檔 (Configuration) 與環境變數
1. 在 `appsettings.json` 裡新增一塊 `EmailSettings` ，裡面有兩個參數 `SmtpServer` 和 `Port`
2. 寫一個類別 `EmailOptions.cs` ， 屬性名稱對應剛剛的 JSON
3. 在 `Program.cs` 中使用 `builder.Services.Configure<EmailOptions>(...)`綁定設定，最後在 API 中用 DI 注入 `IOptions<EmailOptions>` 把數值印在畫面上

[參考資料](https://igouist.github.io/post/2024/08/dotnet-ioptions/)

### 第 9 天：DTO 與資料映射 (Mapping)
1. 建立一個 `User` 實體類別 ( `Id`, `Account`, `Password`, `CreatedDate`)
2. 建立一個 `UserResponseDTO` 類別 (`Account`, `CreatedDate`)
3. 寫一個假資料 API 回傳 `User` 資訊。使用 Mapster 的擴充方法 `.Adapt<UserResponseDTO>()` ，畫面上只看到 `Account` 和 `CreatedDate`

[參考資料](https://dotblogs.com.tw/Null/2020/03/23/221949)

### 第 10 天：資料驗證 (FluentValidation)
1. 建立一個 `RegisterRequestDTO` ( `Username`, `Password`, `Email`, `Age`)
2. 寫一個 `RegisterValidator` 繼承自 `AbstractValidator<RegisterRequestDTO>`
3. 實作以下防護規則
   3.1 帳號不可為空，且長度需在 6~20 字元之間
   3.2 密碼必須包含至少一個大寫字母與數字(使用 `.Matches()`)
   3.3 年齡必須大於等於 18 歲
   3.4 Email 格式必須正確
4. 當前端的資料觸發驗證失敗時，確認 API 有回傳 `400 Bad Request` ，且 JSON 有詳細指出是哪個欄位錯誤

[參考文件](https://docs.fluentvalidation.net/en/latest/aspnet.html)

### 第 11 天：EF Core 基礎與 DB First (反向工程)
1. 執行 `Scaffold-DbContext` 指令，在 Models 資料夾中生成 `BookStoreDbContext.cs` 與 5 張實體類別表
2. 設定 Foreign Key 的欄位會自動生成 public virtual 這個導覽屬性 
3. 在 `appsettings.json` 設定連線字串，在 Program.cs 註冊 DbContext，寫一隻 API 撈取 Books 表的資料

[參考文件](https://learn.microsoft.com/zh-tw/ef/core/dbcontext-configuration/)