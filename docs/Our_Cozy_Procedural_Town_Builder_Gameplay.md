# Gameplay Design: Cozy Procedural Town Builder Của Chúng Ta

## Mục Tiêu Sản Phẩm

Chúng ta sẽ xây dựng một game thuộc nhóm cozy procedural town builder: người chơi tạo thị trấn đẹp, thư giãn, không áp lực, nhưng có nhiều quyền kiểm soát sáng tạo hơn Townscaper.

Game cần giữ tinh thần:

- Dễ chơi trong vài giây đầu.
- Đặt block là thấy đẹp ngay.
- Không cần quản lý tài nguyên phức tạp.
- Không có thất bại nặng.
- Tập trung vào sáng tạo, thư giãn, khám phá và chia sẻ.

## Quan Điểm Thiết Kế Quan Trọng Nhất

Đây trước hết là một game thư giãn. Người chơi không vào game để tối ưu tốc độ xây dựng, hoàn thành nhiệm vụ nhanh, hay sản xuất thật nhiều công trình trong thời gian ngắn. Người chơi vào game để chậm rãi đặt từng khối, quan sát thị trấn thay đổi, thử màu sắc, chỉnh hình dáng và tận hưởng cảm giác một nơi chốn dần hình thành.

Vì vậy, ưu tiên thiết kế của chúng ta là:

- Cảm giác đặt từng block phải dễ chịu.
- Mỗi thao tác nhỏ nên tạo ra phản hồi hình ảnh/âm thanh có giá trị.
- Người chơi phải có thời gian quan sát và cảm nhận công trình.
- Công cụ hỗ trợ không được biến trải nghiệm thành thao tác sản xuất hàng loạt.
- Các chức năng làm nhanh như copy/paste cụm nhà, brush xây nhiều block, nhân bản khu phố chỉ là chức năng phụ, nên để sau MVP.

Điều quan trọng không phải là người chơi xây được nhanh đến đâu, mà là quá trình xây có đủ thư giãn, đẹp, dễ hiểu và đáng quay lại hay không.

Nhưng game của chúng ta cần cải thiện những điểm người dùng Townscaper thường mong muốn:

- Nhiều loại địa hình hơn.
- Nhiều loại grid hơn.
- Có cư dân và sự sống.
- Camera tốt hơn, có street view.
- Mobile controls tốt hơn.
- Công cụ chỉnh sửa an toàn hơn như undo/redo, tránh làm người chơi sợ sai.
- Hệ thống procedural tối ưu hơn.

## Giải Thích "Cozy Procedural Town Builder"

"Cozy procedural town builder" là một game xây dựng thị trấn có 3 đặc điểm chính.

### Cozy

"Cozy" nghĩa là trải nghiệm dễ chịu, thư giãn, ít áp lực.

Trong game của chúng ta, cozy thể hiện qua:

- Không có thua cuộc.
- Không ép thời gian.
- Không bắt người chơi tối ưu tài nguyên.
- Âm thanh nhẹ, phản hồi chạm/click êm.
- Màu sắc dễ chịu.
- Công trình tự động đẹp dù người chơi không chuyên.
- Người chơi có thể vào game 3 phút hoặc 1 tiếng đều hợp lý.

### Procedural

"Procedural" nghĩa là hệ thống tự sinh nội dung bằng quy tắc/thuật toán thay vì bắt người chơi đặt từng chi tiết nhỏ.

Ví dụ:

- Người chơi đặt một block.
- Game tự quyết định block đó nên thành tường, mái, cầu, ban công, cầu thang hay sân.
- Nếu các block tạo vòng kín, game có thể tự sinh sân trong hoặc vườn.
- Nếu nhà gần nước, game có thể tự sinh bến tàu.
- Nếu đường nối nhiều nhà, game có thể tự sinh quảng trường nhỏ hoặc đèn đường.

Procedural không có nghĩa là game chơi thay người chơi. Người chơi vẫn quyết định hình khối, vị trí, màu, vật liệu, phong cách. Thuật toán chỉ giúp biến quyết định đó thành hình ảnh đẹp và nhất quán.

### Town Builder

"Town builder" nghĩa là người chơi xây thị trấn, làng, đảo, khu phố, cảng hoặc thành phố nhỏ.

Nhưng game của chúng ta không phải city-builder quản lý truyền thống. Chúng ta không đặt trọng tâm vào thuế, dân số, điện nước, giao thông hay sản xuất. Trọng tâm là:

- Hình dáng thị trấn.
- Cảm giác nơi chốn.
- Vẻ đẹp kiến trúc.
- Sự sống nhẹ trong môi trường.
- Sáng tạo cá nhân.

## Gameplay Cốt Lõi Của Game Chúng Ta

### Vòng Lặp Chính

1. Người chơi chọn một world seed hoặc loại bản đồ.
2. Người chơi chọn công cụ: xây nhà, địa hình, đường, nước, trang trí.
3. Người chơi chạm/click để đặt block hoặc paint địa hình.
4. Hệ thống procedural tự sinh kiến trúc phù hợp với vị trí.
5. Cư dân, thuyền, chim, ánh sáng và chi tiết môi trường phản ứng theo công trình.
6. Người chơi chỉnh màu, vật liệu, grid, camera hoặc theme.
7. Người chơi chụp ảnh, đi dạo street view, lưu hoặc chia sẻ công trình.

### Trụ Cột Trải Nghiệm

| Trụ cột | Ý nghĩa | Cách triển khai |
| --- | --- | --- |
| Xây dễ, đẹp ngay | Người chơi không cần học nhiều | Một chạm đặt block, procedural tự sinh chi tiết |
| Tự do nhưng có kiểm soát | Người chơi tạo được thứ mình hình dung | Chọn grid, terrain, material, theme |
| Thị trấn có sự sống | Công trình không bị trống vắng | Cư dân nhỏ, thuyền, chim, đèn, hoạt động nhẹ |
| Không áp lực | Giữ tinh thần cozy | Không thất bại, không tài nguyên bắt buộc |
| Tối ưu mobile | Chơi tốt trên điện thoại | Gesture rõ, camera pivot đúng điểm chạm, undo nhanh |
| Dễ chia sẻ | Công trình đẹp nên cần lan truyền | Photo mode, postcard export, seed/share code |

## Nội Dung App Của Chúng Ta

### Bối Cảnh

Game có thể bắt đầu với bối cảnh là thị trấn đảo ven biển giống tinh thần Townscaper, sau đó mở rộng sang nhiều theme:

- Coastal town: thị trấn biển nhiều màu.
- Garden town: thị trấn nhiều cỏ, sân vườn, công viên.
- Canal town: kênh nước, cầu, thuyền.
- Hill town: nhà bậc thang trên đồi.
- Harbor town: bến cảng, thuyền, kho gỗ, hải đăng.
- Old town: quảng trường đá, tháp chuông, đường hẹp.

### Nội Dung Chính

- Nhà và block kiến trúc.
- Đường, cầu, bến nước.
- Địa hình nước, đất, cỏ, đá, cát.
- Vườn, cây, công viên nhỏ.
- Cư dân và hoạt động nhẹ.
- Thuyền và chuyển động môi trường.
- Ánh sáng ngày/đêm.
- Photo mode và street view.
- Seed, save slot, gallery.

## Chức Năng Tương Tự Townscaper

| Chức năng | Cách giống Townscaper | Điểm mạnh | Điểm yếu/Rủi ro |
| --- | --- | --- | --- |
| Đặt/xóa block | Một hành động chính để xây | Dễ hiểu, dễ chơi, giữ cảm giác thư giãn | Nếu chỉ có vậy sẽ dễ bị xem là clone |
| Procedural architecture | Tự sinh nhà, mái, cầu, vòm theo ngữ cảnh | Tạo cảm giác kỳ diệu, giảm công cho người chơi | Cần nhiều rule để tránh kết quả lặp lại |
| Palette màu nhanh | Chọn màu trước khi xây | UI gọn, thao tác nhanh | Cần mở rộng để không bị giới hạn như Townscaper |
| Sandbox không áp lực | Không ép nhiệm vụ chính | Giữ nhóm người chơi cozy | Có thể thiếu động lực lâu dài |
| Camera orbit/zoom | Quan sát công trình từ bên ngoài | Phù hợp xây dựng | Cần thêm street view để khác biệt |
| Save/load | Lưu thị trấn | Thiết yếu | Cần UX tốt nếu có nhiều world |

## Chức Năng Mới Khác Townscaper

| Chức năng mới | Mô tả | Điểm mạnh | Điểm yếu/Rủi ro |
| --- | --- | --- | --- |
| Chọn loại grid | Organic, square, circle, canal, hill, island seed | Giải quyết nhu cầu lớn của người dùng; tăng khả năng sáng tạo | Tăng độ phức tạp procedural và UI |
| Terrain brush | Paint nước, đất, cỏ, đá, cát, đường lát | Mở rộng hình ảnh, không chỉ xây trên nước | Nếu quá nhiều layer sẽ làm mất sự đơn giản |
| Material system | Chọn vật liệu tường, mái, nền, đường | Tạo khác biệt visual lớn | Cần kiểm soát art direction để không rối |
| Color wheel + palette | Vừa có palette nhanh vừa có tùy chỉnh sâu | Phù hợp cả casual và creator | UI màu có thể phức tạp trên mobile |
| Cư dân nhẹ | Người nhỏ đi bộ, đứng ở quảng trường, vào nhà | Thị trấn có sự sống, đáp ứng mong muốn phổ biến | Pathfinding và performance cần tối ưu |
| Thuyền và hoạt động nước | Thuyền đi theo kênh/bến | Làm thế giới sinh động, hợp bối cảnh ven nước | Cần hệ thống route đơn giản, tránh kẹt |
| Street view | Đi dạo trong thị trấn ở góc nhìn người | Khác biệt mạnh, tăng cảm xúc sở hữu công trình | Camera collision và scale kiến trúc phải tốt |
| Photo/postcard mode | Chụp ảnh với filter, time of day, FOV | Tăng chia sẻ xã hội | Không nên làm UI quá nặng |
| Undo/redo nhiều bước | Quay lại thao tác xây | Giảm sợ sai, cần cho mobile | Cần lưu lịch sử hiệu quả |
| Copy/paste cụm nhà | Chọn một vùng nhỏ để nhân bản | Hữu ích cho creator nâng cao sau này | Không quan trọng với trải nghiệm thư giãn cốt lõi; có thể làm giảm cảm giác organic nếu lạm dụng |
| Brush xây nhiều block | Kéo để đặt đường/dãy nhà | Hữu ích khi công trình rất lớn | Không nên ưu tiên sớm vì dễ biến nhịp chơi thành xây hàng loạt |
| Soft challenge | Daily prompt, seed of the day, theme challenge | Tạo động lực mà không phá cozy | Không được biến thành nhiệm vụ ép buộc |
| Chunk-based rebuild | Chỉ cập nhật vùng bị ảnh hưởng | Tối ưu hơn khi thị trấn lớn | Cần thiết kế dữ liệu ngay từ đầu |

## Phân Tích Mạnh/Yếu Theo Nhóm Chức Năng

### 1. Xây Dựng Block

Điểm mạnh:

- Là hành động đơn giản nhất.
- Dễ làm tutorial.
- Phù hợp mobile.
- Tạo phản hồi tức thì.

Điểm yếu:

- Nếu không có biến thể, dễ lặp lại.
- Nếu đặt từng block mãi, xây thành phố lớn sẽ chậm.

Giải pháp:

- Giữ một chạm làm hành động chính.
- Ưu tiên undo/redo vì nó giảm sợ sai mà không làm mất nhịp thư giãn.
- Brush và copy/paste chỉ là công cụ phụ cho giai đoạn sau.
- Không bắt người chơi mới dùng công cụ nâng cao.

### 2. Procedural Architecture

Điểm mạnh:

- Tạo khác biệt cốt lõi.
- Làm người chơi cảm thấy hệ thống thông minh.
- Cho kết quả đẹp mà không cần thao tác chi tiết.

Điểm yếu:

- Khó debug.
- Dễ sinh kết quả không đúng ý.
- Cần nhiều rule để tránh nhà nhìn giống nhau.

Giải pháp:

- Tách procedural theo module: roof, wall, bridge, stairs, courtyard, waterfront, decoration.
- Có preview trước khi đặt.
- Cho người chơi override nhẹ bằng material/theme.

### 3. Grid Và Terrain

Điểm mạnh:

- Đây là cơ hội cải tiến rõ nhất so với Townscaper.
- Tạo replay value lớn.
- Mỗi loại grid cho một phong cách thị trấn khác nhau.

Điểm yếu:

- Tăng độ phức tạp thuật toán.
- Một số grid có thể làm procedural khó tạo kiến trúc đẹp.

Giải pháp:

- MVP chỉ cần 3 loại: organic island, square town, canal town.
- Mỗi grid có rule riêng thay vì cố dùng một rule cho tất cả.

### 4. Sự Sống Trong Thị Trấn

Điểm mạnh:

- Giải quyết cảm giác trống vắng.
- Tăng cảm xúc và khả năng quay lại.
- Rất dễ nhìn thấy trong trailer/screenshot.

Điểm yếu:

- Pathfinding tốn hiệu năng.
- Cư dân có thể khiến người chơi kỳ vọng gameplay mô phỏng sâu hơn.

Giải pháp:

- Cư dân chỉ là ambient, không phải hệ thống kinh tế.
- Dùng path đơn giản trên đường/khu vực hợp lệ.
- Spawn theo mật độ nhà và quảng trường.

### 5. Camera Và Khám Phá

Điểm mạnh:

- Street view là khác biệt lớn.
- Tăng giá trị công trình sau khi xây.
- Hỗ trợ người chơi tạo nội dung chia sẻ.

Điểm yếu:

- Cần xử lý collision, clipping, scale.
- Nếu camera khó dùng trên mobile sẽ gây khó chịu.

Giải pháp:

- Có 3 mode rõ: Build View, Photo View, Walk View.
- Gesture mobile có thể tùy chỉnh.
- Camera xoay quanh điểm chạm khi xây.

### 6. Mục Tiêu Mềm

Điểm mạnh:

- Giảm cảm giác chán.
- Tạo lý do quay lại mỗi ngày.
- Không phá tinh thần sandbox nếu làm đúng.

Điểm yếu:

- Nếu làm quá giống nhiệm vụ, game mất chất cozy.
- Nếu reward quá mạnh, người chơi sẽ tối ưu thay vì sáng tạo.

Giải pháp:

- Daily prompt chỉ là gợi ý.
- Không khóa chức năng chính sau progression.
- Achievement nên ghi nhận sáng tạo, không ép grind.

## MVP Đề Xuất

### Bắt Buộc Có

- Xây/xóa block.
- 3 màu hoặc nhiều hơn trong palette.
- Procedural sinh nhà, mái, cầu, vòm cơ bản.
- Organic island grid.
- Save/load một hoặc nhiều thị trấn.
- Camera orbit/zoom/pan.
- Undo/redo.
- Photo mode đơn giản.

### Nên Có Trong Bản Đầu Nếu Kịp

- Terrain cơ bản: nước, đất, cỏ, đường đá.
- Cư dân ambient đơn giản.
- Street view cơ bản.
- 2 loại grid: organic và square/canal.
- Color wheel hoặc palette mở rộng.

### Để Sau MVP

- Copy/paste cụm nhà.
- Brush xây nhiều block.
- Nhiều theme kiến trúc.
- Thuyền route.
- Daily challenge.
- Gallery/share code.
- Export 3D model.
- Mod/custom palette.

## Nguyên Tắc Không Nên Phá

- Không biến game thành quản lý tài nguyên nặng.
- Không ép người chơi làm nhiệm vụ để mở khóa chức năng cơ bản.
- Không để UI che mất trải nghiệm xây.
- Không thêm quá nhiều thông số ngay từ đầu.
- Không hy sinh cảm giác đặt block mượt.
- Không để procedural quá ngẫu nhiên khiến người chơi mất kiểm soát.
- Không ưu tiên công cụ làm nhanh hơn cảm giác thư giãn của từng thao tác xây dựng.
- Không để copy/paste, brush hàng loạt hoặc các công cụ creator nâng cao trở thành kỳ vọng chính của gameplay ban đầu.

## Định Vị Khác Biệt So Với Townscaper

Townscaper là một món đồ chơi xây thị trấn cực kỳ tối giản.

Game của chúng ta nên là một món đồ chơi xây thị trấn vẫn tối giản, nhưng sâu hơn ở khả năng sáng tạo:

- Townscaper cho người chơi đặt block trên một thế giới đẹp.
- Game của chúng ta cho người chơi chọn loại thế giới, chất liệu, mặt đất, nhịp sống và góc nhìn khám phá.

Thông điệp sản phẩm có thể là:

"Build a living little town, block by block, then step inside and wander through it."
