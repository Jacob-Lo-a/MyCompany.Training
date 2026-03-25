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

### 第 12 天：NLog 導入與擊敗 N+1 效能問題
1. 在 Nuget 安裝 `NLog.Web.AspNetCore` 套件
2. 在 Training.API 根目錄新增 `nlog.config` (XML檔)，把屬性->複製到輸出目錄 改為 有更新時才複製(Copy if newer)
3. 寫一隻 API 撈取所有 Author，在迴圈裡存取 Author.Books 屬性
4. 打開 logs/spl-trace.txt 觀察 NLog 補捉到的 SQL 查詢

[參考文件](https://ironpdf.com/zh-hant/blog/net-help/nlog-csharp-guide/)

### 第 13 天：效能優化與資料庫交易 (Transaction)
1. 寫一個查詢書籍的 API ，並在 EF Core 語句加上 `.AsNoTracking()`
2. 寫一個買書 API
   2.1 扣除 `Books` 庫存
   2.2 新增 `Orders` 紀錄
   2.3 使用 `Transaction` 保護
3. 故意造成錯誤，並去資料庫檢查是否成功 `Rollback`

[參考文件](https://learn.microsoft.com/zh-tw/aspnet/web-forms/overview/data-access/working-with-batched-data/wrapping-database-modifications-within-a-transaction-cs)

### 第 14 天：3-Tier 三層式架構與 Repository 模式
1. 建立 `IBookRepository` ，裡面只有 `_dbContext` 去查詢資料庫的方法
2. 建立 `IBookService` ，建構子注入 `IBookRepository`。實作一個方法：呼叫 `Repository` 拿到書本後，寫C#邏輯把所有書本打 8 折
3. Controller 建構子中注入 `IBookService`。 Controller 只能呼叫 Service 拿資料並回傳

[參考文件](https://igouist.github.io/post/2021/10/newbie-5-3-layer-architecture/)

### 第 15 天：進階查詢、分頁與 try-catch 防呆機制
1. 實作 `GetBooks` API ，使用 `IQueryable` 接收分頁與關鍵參數，並實作動態的 `.Where()` 過濾
2. 在 API　內包覆 try-catch。捕捉特定的 `OperationCanceledException`，並用 NLog 紀錄警告 `_logger.LogWarning`(使用者取消書籍查詢)
3. 捕捉通用的 Exception 。若拋出不可預期的錯誤，在 catch 紀錄 LogError，並統一回傳 `500 Internal Server Error` 給前端

[參考文件](https://comate.baidu.com/zh/page/7pi2mt2gc3s#5)

### 第 16 天：JWT 身分驗證與 3-Tier 會員模組
1. 建立 `IUserRepository` 與 `IUserService`。在 Service 實作檢查帳號密碼的邏輯，若密碼錯誤，請直接 throw new UnauthorizedAccessException("帳號或密碼錯誤")
2. 寫一個 /api/auth/login API。呼叫 Service 驗證成功後，使用 `JwtSecurityTokenHandler` 產出一組效期為 1 小時的 JWT 字串，並在 Token 內塞入使用者的 Role (例如 Admin)
3. 將 Day 14 寫好的 /api/books (新增書籍 API) 加上 [Authorize(Roles = "Admin")]，驗證只有管理員能上架新書。

[參考文件](https://eandev.com/post/software/aspnet-core-authenticaiton-jwt/)

### 第 17 天：全域異常處理與 Swagger 完善
1. 把 Day 15 中 Controller 裡的 try-catch 全部拔掉
2. 實作 `GlobalExceptionHandler.cs` 並繼承介面 `IExceptionHandler`，攔截到錯誤時，使用 Day 12 設定好的 NLog 記錄 `_logger.LogError(ex, "系統發生未預期錯誤")`，並將狀態碼轉為 HTTP 500 回傳 `ProblemDetails` 格式的 JSON
3. 在 Program.cs 中設定 SwaggerGen 加入 `JWT AddSecurityDefinition`。為「書籍查詢 API」加上 `<summary>` 註解

[參考文件SwaggerGen](https://igouist.github.io/post/2021/10/swagger-enable-authorize/)
[參考文件IExceptionHandler](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-10.0)

### 第 18 天：HttpClientFactory、Polly 與外部依賴
1. 安裝 Polly 套件 Microsoft.Extensions.Http.Polly
2. 使用 `HttpClientFactory` 呼叫外部的 API，寫一個 API (TaipeiWeather) 呼叫中央氣象屬的資料，取得台北市未來三天的天氣預報
3. 實作 Polly 自動重試 (Retry)。失敗了等 2 秒再試，試了 3 次都不行，才拋出 Exception 讓全域守門員處理

[參考文件Polly](https://www.nuget.org/packages/microsoft.extensions.http.polly/)
[參考文件IHttpClientFactory](https://learn.microsoft.com/zh-tw/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)