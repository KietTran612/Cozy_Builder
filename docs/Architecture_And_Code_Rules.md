# Architecture And Code Rules

## Mục Đích

Tài liệu này chốt các quyết định kỹ thuật nền tảng cho dự án Unity + URP:

- Dùng VContainer ngay từ đầu.
- Áp dụng DIP nghiêm túc.
- Dùng `struct` cho dữ liệu gameplay compact.
- Dùng UniTask cho workflow async.

Các rule này nhằm giữ code sạch, dễ test, tối ưu performance, giảm GC, và tránh phải refactor kiến trúc lớn khi game bước từ prototype sang production.

## Rule Tổng Quát

- Data logic không phụ thuộc vào GameObject.
- Logic chính nằm trong pure C# class/service, không nhét hết vào MonoBehaviour.
- MonoBehaviour chủ yếu là adapter cho Unity lifecycle, input, view, scene object.
- Dependency được tạo ở composition root bằng VContainer.
- Không dùng singleton tĩnh cho service gameplay.
- Không dùng `FindObjectOfType`, `GameObject.Find`, `GetComponent` lặp lại trong hot path.
- Không resolve dependency trong gameplay hot path.
- Không dùng async/await hoặc DI để thay thế data-oriented design.

## 1. VContainer

### Quyết Định

Dự án sẽ dùng VContainer ngay từ đầu làm dependency injection container cho Unity.

Nguồn:

- https://github.com/hadashiA/VContainer

### VContainer Dùng Để Làm Gì

VContainer giúp:

- Tạo và quản lý dependency giữa các service.
- Inject dependency vào class thông qua constructor.
- Inject dependency vào MonoBehaviour khi cần.
- Quản lý lifetime: Singleton, Scoped, Transient.
- Tạo composition root rõ ràng cho scene/game.
- Giảm nhu cầu dùng singleton tĩnh.
- Giúp code dễ test vì service phụ thuộc interface/class rõ ràng.

### Khái Niệm Chính

#### LifetimeScope

`LifetimeScope` là nơi đăng ký dependency cho một scene hoặc một phạm vi chạy.

Ví dụ:

```csharp
public sealed class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlacementService>(Lifetime.Singleton);
        builder.Register<RuleEvaluator>(Lifetime.Singleton);
        builder.Register<ChunkRebuilder>(Lifetime.Singleton);
        builder.Register<LocalTownRepository>(Lifetime.Singleton)
            .As<ITownRepository>();
    }
}
```

#### Register

`Register` khai báo cách tạo một service.

Ví dụ:

```csharp
builder.Register<PlacementService>(Lifetime.Singleton);
```

#### As Interface

Đăng ký implementation dưới interface để tuân thủ DIP.

```csharp
builder.Register<LocalTownRepository>(Lifetime.Singleton)
    .As<ITownRepository>();
```

#### Lifetime

- `Singleton`: một instance trong scope.
- `Scoped`: một instance theo scope.
- `Transient`: tạo mới mỗi lần resolve.

Với dự án này, phần lớn service gameplay nên là `Singleton` trong game scene scope.

Quy ước lifetime cho dự án:

- `Singleton`: service hệ thống trong một game scene, ví dụ `PlacementService`, `RuleEvaluator`, `SaveService`.
- `Scoped`: chỉ dùng khi có phạm vi rõ, ví dụ một gameplay session/town session riêng.
- `Transient`: hạn chế dùng trong runtime mobile; chỉ dùng cho object nhẹ, ít tạo, không nằm trong hot path.

Không dùng `Transient` cho object được tạo liên tục như cell, prop, character hoặc command hàng loạt.

### Nên Dùng VContainer Cho

- `PlacementService`
- `RuleEvaluator`
- `ChunkRebuilder`
- `TownDataStore`
- `SaveService`
- `AssetLoadingService`
- `AudioService`
- `HapticService`
- `CameraService`
- `PhotoModeService`
- `PurchaseService`
- `AnalyticsService`
- `SettingsService`
- Scene/bootstrap flow

### Không Dùng VContainer Cho

- `CellData`
- `GridCoord`
- `ChunkCoord`
- `BuildCommand`
- `RuleResult`
- Object tạo/xóa hàng loạt trong gameplay.
- Từng block, từng cell, từng prop nhỏ.
- Mỗi cư dân/thuyền/chim nếu số lượng nhiều.
- Logic cần chạy mỗi frame trong hot path.

### Rule Bắt Buộc Khi Dùng VContainer

- Chỉ cấu hình dependency trong `LifetimeScope`.
- Không gọi `Resolve()` lung tung trong gameplay code.
- Không dùng VContainer như service locator.
- Không inject mọi thứ chỉ vì có thể.
- Không để class tự đăng ký dependency.
- Không resolve trong `Update`, placement hot path, rebuild chunk loop.
- Không dùng DI cho data object nhỏ.
- Không dùng VContainer để thay pooling.
- Không trộn nhiều composition root không rõ phạm vi.
- Không inject trực tiếp vào prefab/object được spawn hàng loạt nếu pooling hoặc factory rõ ràng phù hợp hơn.
- Nếu scene có nhiều `LifetimeScope`, phải ghi rõ scope nào sở hữu service nào.
- Nếu cần tạo object runtime bằng VContainer, phải dùng factory có kiểm soát và không gọi trong hot path.

### Cách Tổ Chức Đề Xuất

```text
GameLifetimeScope
    registers:
        TownDataStore
        PlacementService
        RuleEvaluator
        ChunkRebuilder
        SaveService
        CameraService
        AudioService
```

Pure C# service:

```csharp
public sealed class PlacementService
{
    private readonly TownDataStore townData;
    private readonly RuleEvaluator ruleEvaluator;
    private readonly ChunkRebuilder chunkRebuilder;

    public PlacementService(
        TownDataStore townData,
        RuleEvaluator ruleEvaluator,
        ChunkRebuilder chunkRebuilder)
    {
        this.townData = townData;
        this.ruleEvaluator = ruleEvaluator;
        this.chunkRebuilder = chunkRebuilder;
    }
}
```

MonoBehaviour adapter:

```csharp
public sealed class PlacementInputView : MonoBehaviour
{
    [Inject] private PlacementService placementService;

    private void Update()
    {
        // Read Unity input, call placementService.
    }
}
```

Với MonoBehaviour, chỉ inject vào adapter/view cần gọi service. Không biến mọi component thành nơi chứa logic nghiệp vụ.

Nếu cần khởi tạo sau inject, dùng pattern lifecycle rõ ràng thay vì dựa vào thứ tự `Awake` không kiểm soát được.

### Ưu Điểm

- Code sạch hơn.
- Dễ test service độc lập.
- Giảm singleton tĩnh.
- Dễ thay implementation.
- Hợp với dự án dài hạn.
- Dễ mở rộng khi có save, purchase, analytics, asset loading.

### Nhược Điểm/Rủi Ro

- Thêm dependency bên thứ ba.
- Cần học và dùng đúng `LifetimeScope`.
- Nếu dùng sai sẽ thành service locator trá hình.
- Nếu inject quá nhiều vào MonoBehaviour, code vẫn rối.
- Nếu resolve trong hot path, có thể gây overhead và khó debug.
- Sai scope/lifetime có thể gây bug khó tìm, ví dụ service scene cũ còn giữ reference tới object đã destroy.

### Kết Luận

VContainer được dùng ngay từ đầu, nhưng chỉ dùng để quản lý dependency cấp hệ thống. Nó không được dùng cho dữ liệu cell/block, object hàng loạt, hoặc hot path gameplay.

## 2. DIP - Dependency Inversion Principle

### Quyết Định

Dự án áp dụng DIP ngay từ đầu. VContainer chỉ là công cụ hỗ trợ; nguyên tắc kiến trúc vẫn là phần quan trọng hơn.

### Ý Nghĩa

DIP nghĩa là:

- Module cấp cao không phụ thuộc trực tiếp vào module cấp thấp.
- Cả hai phụ thuộc vào abstraction khi có boundary rõ ràng.
- Implementation chi tiết có thể thay đổi mà không phá logic cấp cao.

### Nên Dùng Interface Cho

- Save/load boundary.
- Purchase boundary.
- Analytics boundary.
- Asset loading boundary.
- Audio/haptic boundary nếu cần đổi implementation.
- Platform service.
- File system/cloud service.

Ví dụ:

```csharp
public interface ITownRepository
{
    UniTask<TownData> LoadAsync(string id, CancellationToken ct);
    UniTask SaveAsync(TownData data, CancellationToken ct);
}
```

```csharp
public sealed class LocalTownRepository : ITownRepository
{
    public UniTask<TownData> LoadAsync(string id, CancellationToken ct)
    {
        // Load from local storage.
    }

    public UniTask SaveAsync(TownData data, CancellationToken ct)
    {
        // Save to local storage.
    }
}
```

### Không Cần Interface Cho

- Class không có implementation thay thế.
- Rule nội bộ đơn giản.
- Data container.
- Utility class chỉ dùng một nơi.
- Service chưa có boundary rõ.

Không tạo interface chỉ để "cho đúng pattern".

### Rule Bắt Buộc Cho DIP

- Service cấp cao không tự `new` dependency phức tạp.
- Không gọi singleton global trong logic chính.
- Không dùng `FindObjectOfType` để lấy service.
- Boundary external/platform nên có interface.
- Pure gameplay rule nên nhận data input rõ ràng, trả output rõ ràng.
- Interface phải phục vụ testability hoặc khả năng thay implementation.

### Ưu Điểm

- Dễ test.
- Dễ thay local/cloud save.
- Dễ mock purchase/analytics.
- Dễ giữ gameplay core không phụ thuộc Unity API.

### Nhược Điểm/Rủi Ro

- Quá nhiều interface làm code rườm rà.
- Overengineering nếu interface không có lý do.
- Có thể khiến người mới khó lần code hơn.

### Kết Luận

Áp dụng DIP nghiêm túc nhưng thực dụng. Interface chỉ dùng ở boundary có lý do rõ.

## 3. Struct Cho Data

### Quyết Định

Dự án dùng `struct` cho dữ liệu gameplay nhỏ, nhiều, compact và thường xuyên được duyệt.

Nguồn chính thống:

- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
- https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/structs
- https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct

### Mục Tiêu

- Giảm số lượng object nhỏ trên heap.
- Giảm GC pressure.
- Lưu dữ liệu town trong array/list liên tục.
- Tăng cache locality khi duyệt nhiều cell.
- Giúp save/load và undo/redo dễ kiểm soát.
- Tách dữ liệu logic khỏi visual GameObject.

### Nên Dùng Struct Cho

- `GridCoord`
- `ChunkCoord`
- `CellIndex`
- `CellData`
- `BlockState`
- `RuleResult`
- `BuildDelta`
- `BuildCommandData`
- `MaterialId`
- `ColorId`
- `TerrainId`

Ví dụ:

```csharp
public readonly struct GridCoord : IEquatable<GridCoord>
{
    public readonly int X;
    public readonly int Y;

    public GridCoord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(GridCoord other)
    {
        return X == other.X && Y == other.Y;
    }
}
```

```csharp
public struct CellData
{
    public ushort Height;
    public ushort ColorId;
    public ushort MaterialId;
    public byte TerrainId;
    public CellFlags Flags;
}
```

```csharp
[Flags]
public enum CellFlags : byte
{
    None = 0,
    Occupied = 1 << 0,
    HasRoad = 1 << 1,
    HasWaterfront = 1 << 2,
    Dirty = 1 << 3
}
```

### Không Dùng Struct Cho

- Service.
- Manager.
- Class có lifecycle phức tạp.
- Object cần identity/reference lâu dài.
- Object lớn có nhiều field.
- Object chứa nhiều reference type.
- MonoBehaviour hoặc wrapper quanh Unity object.

### Rule Bắt Buộc Khi Dùng Struct

- Struct phải nhỏ.
- Ưu tiên primitive field: `byte`, `ushort`, `int`, enum nhỏ.
- Dùng `readonly struct` cho value object bất biến.
- Không dùng nhiều `bool`; ưu tiên `byte Flags`.
- Implement `IEquatable<T>` nếu so sánh nhiều.
- Tránh boxing qua interface/object.
- Không truyền struct lớn by value trong hot path.
- Nếu struct lớn hơn mức nhỏ/gọn, cân nhắc truyền bằng `in`/`ref` hoặc tách nhỏ data.
- Không dùng mutable struct phức tạp trong API dễ gây copy bug.
- Không giả định struct luôn nằm trên stack.
- Không dùng property phức tạp trong struct nếu nó che giấu logic nặng.
- Không lưu reference tới Unity object trong struct data gameplay.

### Về StructLayout

Không dùng `[StructLayout(LayoutKind.Auto)]` nếu cần layout ổn định cho binary save/native interop.

Nếu cần kiểm soát layout, cân nhắc:

```csharp
[StructLayout(LayoutKind.Sequential)]
public struct CellData
{
    public ushort Height;
    public ushort ColorId;
    public ushort MaterialId;
    public byte TerrainId;
    public CellFlags Flags;
}
```

Nhưng với save/load, không nên phụ thuộc trực tiếp vào binary memory layout nếu chưa thật cần. Có thể serialize rõ field theo version để dễ migration.

Với `CellData`, ưu tiên serialize theo schema/version thay vì ghi raw bytes trực tiếp. Raw binary chỉ nên dùng khi đã chốt format và có test migration.

### Ưu Điểm

- Rất hợp với dữ liệu cell/block số lượng lớn.
- Giảm allocation.
- Dễ lưu trong array.
- Hợp với chunk rebuild và procedural evaluation.

### Nhược Điểm/Rủi Ro

- Copy-by-value có thể gây bug.
- Struct lớn copy tốn CPU.
- Boxing có thể làm mất lợi ích.
- Mutable struct cần dùng rất cẩn thận.

### Kết Luận

Dùng struct mạnh cho data, nhưng không biến mọi thứ thành struct. Service và logic vẫn dùng class.

## 4. UniTask

### Quyết Định

Dự án dùng UniTask cho workflow async thay coroutine truyền thống ở các luồng phù hợp.

Nguồn:

- https://github.com/Cysharp/UniTask
- https://cysharp.github.io/UniTask/

### UniTask Dùng Để Làm Gì

UniTask cung cấp async/await tối ưu cho Unity:

- Await Unity AsyncOperation.
- Delay theo PlayerLoop.
- Workflow async ít allocation hơn `Task`.
- Cancellation tốt hơn coroutine truyền thống.
- Tổ chức code loading/save/UI flow rõ hơn.

### Nên Dùng UniTask Cho

- Load scene.
- Load asset/addressables.
- Save/load town.
- Autosave.
- Purchase flow.
- Restore purchase.
- Analytics gửi sự kiện.
- Tutorial sequence.
- UI transition.
- Photo/export flow.
- UnityWebRequest.
- Delay/time-based flow không nằm trong hot path.

Ví dụ:

```csharp
public async UniTask SaveTownAsync(TownData data, CancellationToken ct)
{
    await repository.SaveAsync(data, ct);
}
```

### Không Dùng UniTask Cho

- Placement hot path.
- Cell neighbor calculation.
- Procedural rule evaluation từng block.
- Chunk mesh rebuild CPU-heavy nếu đang chạy sync tốt hơn.
- Logic cần chạy mỗi frame mà `Update` rõ ràng hơn.
- Từng cư dân/thuyền nếu số lượng nhiều và chạy liên tục.

### Rule Bắt Buộc Khi Dùng UniTask

- Mọi UniTask dài phải nhận `CancellationToken`.
- MonoBehaviour async phải cancel khi object destroy.
- Không dùng `async void`, trừ event handler bắt buộc.
- Hạn chế `UniTaskVoid`; chỉ dùng cho fire-and-forget có kiểm soát.
- Fire-and-forget phải dùng có chủ đích và log exception.
- Không bỏ qua exception trong async flow.
- Không dùng UniTask để che logic blocking nặng trên main thread.
- Không gọi async workflow từ hot path mỗi frame nếu không cần.
- Không trộn coroutine và UniTask cho cùng một flow trừ khi có lý do rõ.
- Không gọi `.Forget()` nếu không có handler/log lỗi.
- Không dùng `Task.Delay` cho gameplay timing; dùng UniTask delay theo PlayerLoop khi cần.

Ví dụ cancellation:

```csharp
public sealed class TownLoaderView : MonoBehaviour
{
    private void Start()
    {
        LoadAsync(this.GetCancellationTokenOnDestroy()).Forget(Debug.LogException);
    }

    private async UniTask LoadAsync(CancellationToken ct)
    {
        await UniTask.Yield(ct);
        // Load data, update UI, then exit.
    }
}
```

Nếu flow cần await từ service khác, ưu tiên trả về `UniTask` để caller có thể await/cancel. Chỉ fire-and-forget ở tầng view/lifecycle khi thật sự không cần kết quả.

### Ưu Điểm

- Code async dễ đọc.
- Tốt cho loading/save/purchase.
- Có cancellation rõ ràng.
- Hợp Unity PlayerLoop.
- Ít allocation hơn `Task`.

### Nhược Điểm/Rủi Ro

- Thêm dependency bên thứ ba.
- Debug async flow cần kinh nghiệm.
- Dễ leak logic nếu quên cancellation.
- Dễ nuốt exception nếu fire-and-forget sai.
- Không thay thế được tối ưu thuật toán.
- Async không tự chuyển việc nặng ra background. Nếu logic CPU-heavy vẫn chạy trên main thread thì vẫn gây lag.

### Kết Luận

UniTask được dùng cho workflow async, không dùng để thay thế core gameplay loop hoặc procedural hot path.

## Quy Tắc Kết Hợp 4 Thành Phần

### Placement Flow

- Input MonoBehaviour nhận tap.
- MonoBehaviour gọi `PlacementService`.
- `PlacementService` là class được inject bằng VContainer.
- `PlacementService` xử lý `CellData`/`GridCoord` dạng struct.
- `RuleEvaluator` nhận data, trả `RuleResult` struct.
- `ChunkRebuilder` mark dirty chunk.
- Không dùng UniTask trong thao tác đặt block bình thường.

### Save Flow

- `SaveService` được inject bằng VContainer.
- `SaveService` phụ thuộc `ITownRepository` theo DIP.
- `TownData` chứa array/list struct data.
- Save/load dùng UniTask có cancellation.
- Save format có version.

### Asset Loading Flow

- `AssetLoadingService` được inject bằng VContainer.
- Boundary có interface nếu cần mock/test.
- Load asset dùng UniTask.
- Asset runtime không được resolve trong hot path.

## Checklist Bắt Buộc Khi Review Code

- Class này có cần inject không, hay chỉ là data?
- Dependency có được đăng ký trong `LifetimeScope` không?
- Có `Resolve()` trong gameplay code không?
- Có singleton tĩnh không?
- Interface này có lý do thật không?
- Struct này có quá lớn không?
- Struct này có bị boxing không?
- Có dùng `bool` rải rác thay vì flags không?
- Async method có `CancellationToken` không?
- Có `async void` không?
- Có dùng UniTask trong hot path không?
- Có allocation trong `Update` không?
- Có phụ thuộc Unity API trong pure gameplay rule không?

## Kết Luận

Quyết định cuối:

- Dùng VContainer ngay từ đầu, nhưng chỉ cho dependency cấp hệ thống.
- Áp dụng DIP thực dụng, không tạo interface vô nghĩa.
- Dùng struct cho data nhỏ/nhiều/compact.
- Dùng UniTask cho async workflow có cancellation.

Các quyết định này bổ trợ nhau, nhưng không thay thế nguyên tắc performance chính: data-first, chunk rebuild, pooling, shared material, và profiling trên thiết bị thật.
