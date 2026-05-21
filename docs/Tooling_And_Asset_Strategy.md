# Kế Hoạch Chọn Công Cụ Và Nguồn Tài Nguyên

## Vấn Đề Thực Tế

Hiện tại dự án không có artist riêng, nên rủi ro lớn không chỉ nằm ở code gameplay mà còn nằm ở visual production. Một game kiểu cozy procedural town builder phải đẹp, nhất quán và dễ chụp screenshot. Nếu asset không hợp style, không modular, license không rõ hoặc quá nặng cho mobile, dự án sẽ mất nhiều thời gian sửa về sau.

Vì vậy, trước khi đi sâu vào prototype, cần có một bước riêng để chọn công cụ, chọn nguồn asset và xác định pipeline art tối thiểu.

## Mục Tiêu

- Chọn được công cụ làm việc phù hợp cho mobile.
- Chọn được nguồn asset có thể dùng hợp pháp trong sản phẩm thương mại.
- Chọn được style đủ đẹp nhưng vẫn khả thi khi không có artist.
- Tránh trộn quá nhiều asset pack làm game mất nhất quán.
- Đảm bảo asset có thể dùng cho procedural generation: tường, mái, cầu, vòm, nền, props phải modular hoặc dễ tách.

## Công Cụ Làm Việc Đề Xuất

### Engine

Khuyến nghị: Unity + URP.

Lý do:

- Phù hợp mobile.
- Có Asset Store lớn.
- Dễ tìm low-poly/stylized asset packs.
- Dễ làm tooling/debug scene cho procedural grid.
- Dễ build Android trước, sau đó mở rộng iOS/Steam nếu cần.

Rủi ro:

- URP/material khác nhau giữa các asset pack có thể cần chỉnh lại.
- Nếu dùng nhiều asset từ nhiều nguồn, style và shader dễ không đồng nhất.

### 3D Editing

Khuyến nghị: Blender.

Dùng để:

- Chỉnh scale.
- Tách module.
- Đổi pivot.
- Giảm poly.
- Sửa UV/material đơn giản.
- Tạo vài mesh phụ nếu thiếu.

### Texture/Material

Nguồn nên ưu tiên:

- Poly Haven cho texture/HDRI CC0.
- Asset pack có material đơn giản, ít texture, phù hợp mobile.
- Flat color hoặc stylized low-poly material để dễ đồng bộ.

### Character/Animation

Nguồn nên cân nhắc:

- Mixamo cho animation humanoid royalty-free theo FAQ chính thức của Adobe.
- Low-poly animated people packs nếu cần cư dân nhỏ.

Lưu ý: cư dân trong game này là ambient, không cần animation phức tạp. Ưu tiên người nhỏ, readable từ xa, ít bone, ít material.

## Nguồn Asset Có Thể Xem Xét

### Unity Asset Store

Phù hợp để mua asset thương mại có sẵn. Cần kiểm tra:

- License type.
- Render pipeline compatibility.
- File size.
- Poly count.
- Material count.
- Có modular pieces hay chỉ là prefab nguyên khối.
- Có mobile demo/performance note không.

Ví dụ nguồn tham khảo:

- Low Poly Modular Village trên Unity Asset Store: https://assetstore.unity.com/packages/3d/environments/low-poly-modular-village-lowpoly-medieval-fantasy-series-258073
- Terrific Modular Fantasy Village: https://terrific3d.com/unity-assets/terrific-modular-fantasy-village/
- Meshworks modular environment kits: https://www.meshworks.dev/

### Kenney

Kenney có nhiều asset CC0, dùng tốt cho prototype hoặc một số phần production nếu style phù hợp.

Nguồn/license:

- https://kenney.nl/support

### Poly Haven

Poly Haven phù hợp cho HDRI, texture, một số model. Tất cả asset trên Poly Haven được công bố CC0, có thể dùng cho commercial work.

Nguồn/license:

- https://polyhaven.com/license

### Quaternius

Quaternius có nhiều low-poly game assets miễn phí. Cần kiểm tra từng pack/license trước khi ship.

Nguồn:

- https://quaternius.com/

### Itch.io Asset Packs

Có nhiều pack indie tốt, nhưng license rất khác nhau theo từng creator. Chỉ dùng khi:

- License commercial rõ ràng.
- Có quyền modify.
- Không yêu cầu attribution nếu chúng ta không muốn quản lý credit phức tạp.
- Không có điều khoản cấm mobile/commercial.

Ví dụ tham khảo:

- Low Poly Village by Unco Games: https://uncogames.itch.io/low-poly-village

## Tiêu Chí Chọn Asset Pack Chính

Một asset pack tốt cho dự án này cần đạt:

- Stylized/cozy, không quá realistic.
- Modular: có tường, mái, cửa, cầu, nền, props tách rời.
- Ít material để batching tốt trên mobile.
- Scale nhất quán.
- Có mesh/pivot sạch để procedural đặt tự động.
- Có đủ biến thể mái/tường để tránh lặp.
- License cho phép commercial use.
- Không dùng asset AI không rõ nguồn gốc.
- Không quá giống Townscaper về silhouette và màu sắc.

## Quy Tắc Chọn Style Khi Không Có Artist

Không nên chọn style quá chi tiết vì sẽ khó tự mở rộng.

Nên chọn:

- Low-poly stylized.
- Flat color hoặc hand-painted rất nhẹ.
- Mái, tường, cửa có hình khối rõ.
- Màu ấm, dễ nhìn, tương phản vừa phải.
- Tỷ lệ hơi toy-like để hợp cozy.

Không nên chọn:

- Realistic PBR nặng.
- Medieval quá tối và nhiều chi tiết.
- Asset nhiều shader custom.
- Asset có quá nhiều texture riêng lẻ.
- Asset đẹp trong demo nhưng khó tách module.

## Pipeline Đề Xuất

### Bước 1: Shortlist

Tìm 5-10 asset pack tiềm năng.

Ghi lại:

- Link.
- Giá.
- License.
- Pipeline hỗ trợ: Built-in/URP/HDRP.
- Số lượng prefab/mesh.
- Material count.
- Mobile suitability.
- Có modular hay không.
- Có phù hợp cozy không.

### Bước 2: Test Import

Chọn 2-3 pack tốt nhất để test.

Kiểm tra:

- Import vào Unity URP có lỗi material không.
- Scale có hợp với camera và street view không.
- Mesh có pivot hợp lý không.
- Có thể tách mái/tường/cửa không.
- FPS trên scene mẫu.
- Có bị lệch style với nước/terrain không.

### Bước 3: Style Lock

Chọn 1 pack làm visual foundation.

Quy tắc:

- Không dùng quá 2 pack chính trong MVP.
- Nếu dùng nhiều pack, phải thống nhất lại material/color.
- Các asset phụ phải được chỉnh để đi theo style chính.

### Bước 4: Procedural Compatibility

Tạo test scene:

- 1 cell đơn.
- 2 cell cạnh nhau.
- 4 cell tạo sân.
- Stack 2-4 tầng.
- Cầu/vòm.
- Waterfront.
- Góc cong hoặc lệch.

Nếu asset không thể phục vụ các case này, không nên dùng làm foundation.

## Checklist License Trước Khi Ship

Với mỗi asset, phải lưu:

- Tên asset.
- Creator/publisher.
- Link nguồn.
- Ngày tải/mua.
- License/EULA.
- Có cho commercial use không.
- Có cho modify không.
- Có cần attribution không.
- Có cấm redistribute standalone không.
- Có chứng từ mua nếu là paid asset.

Không đưa asset vào production nếu:

- License không rõ.
- Chỉ cho personal use.
- Không rõ nguồn gốc.
- Tải từ site reupload không chính thức.
- Không chắc có quyền dùng trong mobile commercial game.

## Kết Luận

Bước chọn công cụ và asset phải nằm trước prototype dài. Với dự án không có artist, đây là một phần của chiến lược sản phẩm chứ không chỉ là việc phụ.

Khuyến nghị hiện tại:

- Dùng Unity URP.
- Chọn low-poly stylized modular assets.
- Ưu tiên một asset pack chính thật phù hợp.
- Dùng CC0 assets như Kenney/Poly Haven cho prototype hoặc bổ sung nhỏ.
- Kiểm tra license và procedural compatibility trước khi xây hệ thống thật quanh asset.

