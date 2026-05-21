# Townscaper Gameplay Research

## Mục Đích Tài Liệu

Tài liệu này tổng hợp gameplay, nội dung, chức năng hiện có và mong muốn của người dùng đối với Townscaper. Đây là cơ sở để thiết kế một game cùng nhóm trải nghiệm nhưng có nhiều cải tiến hơn.

Nguồn tham khảo chính:

- Google Play: https://play.google.com/store/apps/details?id=com.OskarStalberg.Townscaper
- Steam: https://store.steampowered.com/app/1291340/Townscaper/
- PC Gamer review: https://www.pcgamer.com/uk/townscaper-review/
- Pocket Tactics review: https://www.pockettactics.com/townscaper/review
- GameDeveloper technical analysis: https://www.gamedeveloper.com/game-platforms/how-townscaper-works-a-story-four-games-in-the-making

## Tổng Quan Townscaper

Townscaper là một game/toy xây dựng thị trấn thư giãn. Bản thân nhà phát hành mô tả rõ rằng game không có mục tiêu, không có gameplay truyền thống, không có nhiệm vụ, không tài nguyên, không thắng thua. Người chơi chỉ xây dựng vì cảm giác đẹp, thư giãn và sáng tạo.

Điểm cốt lõi của Townscaper là:

- Người chơi chọn màu.
- Người chơi đặt hoặc xóa block trên một lưới bất quy tắc.
- Hệ thống procedural tự biến các block thành kiến trúc có hình dáng hợp lý.
- Kết quả có thể là nhà, mái, cầu, cầu thang, vòm, sân trong, đường ven nước, tháp, hải đăng, khu vườn.
- Người chơi học quy luật bằng thử nghiệm thay vì qua tutorial nặng.

Townscaper không cố trở thành city-builder như Cities: Skylines. Nó giống một món đồ chơi kiến trúc tương tác, nơi hệ thống tự động làm phần phức tạp để người chơi chỉ tập trung vào tạo hình.

## Gameplay Cụ Thể

### Vòng Lặp Chính

1. Người chơi nhìn vào mặt nước/lưới xây dựng.
2. Chọn một màu nhà từ palette.
3. Chạm/click vào một ô để đặt block.
4. Nếu đặt ở mực nước, block thường trở thành nền, đường, bến, hoặc phần móng.
5. Nếu đặt chồng lên block khác, hệ thống tạo tầng nhà, mái, tháp hoặc kiến trúc cao tầng.
6. Nếu đặt gần các block khác, hệ thống tự quyết định chi tiết kết nối như cầu, vòm, cầu thang, sân, hàng rào, lối đi.
7. Người chơi xóa block khi muốn chỉnh sửa hình khối.
8. Tiếp tục lặp lại để hình thành thị trấn.

### Loại Tương Tác

- Đặt block.
- Xóa block.
- Chọn màu.
- Xoay camera.
- Zoom camera.
- Lưu/tải công trình.
- Chụp ảnh hoặc dùng photo/view options tùy nền tảng.
- Một số bản có xuất mô hình 3D.

### Cảm Giác Gameplay

Townscaper tạo cảm giác thư giãn nhờ:

- Không có áp lực thời gian.
- Không có thất bại.
- Không cần tối ưu tài nguyên.
- Âm thanh đặt block dễ chịu.
- Kết quả visual xuất hiện ngay lập tức.
- Người chơi có cảm giác mình tạo ra thứ đẹp dù thao tác rất đơn giản.

## Nội Dung Trong Townscaper

### Bối Cảnh

Bối cảnh chính là các thị trấn ven biển hoặc đảo nhỏ trên mặt nước. Hình ảnh gợi cảm giác châu Âu cổ, cảng biển, làng ven kênh, nhà nhiều màu, mái ngói, tháp chuông, cầu và đường đá.

### Thành Phần Kiến Trúc

Các thành phần nổi bật:

- Nhà nhiều màu.
- Mái ngói.
- Tháp cao.
- Hải đăng.
- Cầu và vòm.
- Cầu thang.
- Ban công.
- Sân trong.
- Vườn nhỏ.
- Đường đá.
- Bến nước.
- Nhà trên cọc.
- Thành phố nổi hoặc sky city nếu xây cao và nối các khối.

### Thành Phần Trang Trí

- Chim.
- Bướm.
- Đèn cửa sổ.
- Dây phơi.
- Chi tiết mái, cửa, lan can.
- Thay đổi ánh sáng/ngày đêm ở một số chế độ.

Các chi tiết này giúp thị trấn có sức sống, nhưng vẫn chưa có cư dân thật sự.

## Chức Năng Đang Có

| Chức năng | Mô tả | Điểm mạnh | Điểm yếu |
| --- | --- | --- | --- |
| Đặt/xóa block | Hành động xây dựng chính | Rất dễ học, phù hợp mobile và casual | Ít công cụ nâng cao cho người chơi sáng tạo lâu dài |
| Chọn màu | Chọn màu nhà từ palette | Nhanh, gọn, không gây rối UI | Số màu hạn chế, không có color wheel đầy đủ |
| Lưới bất quy tắc | Grid organic tạo hình thị trấn tự nhiên | Tạo ra đường cong, góc lạ, hình khối đẹp | Người chơi không kiểm soát được loại grid |
| Procedural architecture | Tự sinh mái, cầu, vòm, sân, cầu thang | Là linh hồn của game, tạo cảm giác kỳ diệu | Người chơi không chọn trực tiếp được chi tiết cụ thể |
| Camera orbit/zoom | Quan sát thị trấn từ nhiều góc | Đủ cho xây dựng và chụp ảnh | Người dùng muốn first-person/street view tốt hơn |
| Không mục tiêu | Sandbox hoàn toàn tự do | Thư giãn, không áp lực | Dễ chán với người cần động lực hoặc progression |
| Âm thanh nhẹ | Âm thanh đặt block và môi trường | Tạo cảm giác cozy | Nội dung âm thanh chưa đủ đa dạng cho phiên chơi dài |
| Save/load | Lưu công trình | Cho phép quay lại chỉnh sửa | Chưa phải hệ thống quản lý dự án sâu |
| Visual polish | Phong cách minh họa đẹp | Rất dễ chia sẻ screenshot | Phụ thuộc mạnh vào một style duy nhất |

## Mong Muốn Của Người Dùng

Các review người dùng trên Google Play và cộng đồng thường nhắc đến các mong muốn sau.

### 1. Không Gian Xây Dựng Rộng Hơn

Người dùng muốn:

- Grid rộng hơn.
- Nhiều block hơn.
- Cảm giác xây không bị giới hạn.
- Có thể mở rộng thị trấn tự nhiên khi xây.

Lý do: Townscaper cho cảm giác rất tự do, nên khi chạm giới hạn, người chơi bị hụt hẫng.

### 2. Chọn Loại Grid

Người dùng muốn chọn:

- Grid organic như hiện tại.
- Grid vuông.
- Grid tròn.
- Grid random theo seed.
- Grid theo đảo, kênh, đồi, bờ biển.

Lý do: Grid quyết định hình dáng thành phố. Nếu không chọn được grid, người chơi khó đạt hình ảnh mong muốn.

### 3. Có Người Dân Và Sự Sống

Mong muốn phổ biến:

- Người nhỏ đi bộ.
- Dân cư xuất hiện khi có nhà.
- Thuyền đi trên kênh.
- Hoạt động chợ, bến cảng, công viên.
- Động vật và chi tiết môi trường phong phú hơn.

Lý do: Thị trấn đẹp nhưng dễ có cảm giác trống vắng.

### 4. Nhiều Loại Mặt Đất Và Vật Liệu

Người dùng muốn:

- Cỏ.
- Vườn.
- Đường lát khác nhau.
- Đất liền thay vì chỉ nước.
- Đá, gỗ, gạch, cát.
- Công viên, quảng trường, bến tàu.

Lý do: Hiện tại hình ảnh chủ yếu xoay quanh nhà, đá, nước và mái ngói.

### 5. Nhiều Màu Hơn

Người dùng muốn:

- Color wheel.
- Palette mở rộng.
- Lưu palette riêng.
- Tùy chỉnh màu mái, tường, nền, chi tiết.

Lý do: Người chơi sáng tạo thường cần kiểm soát màu tốt hơn.

### 6. Camera Tốt Hơn

Người dùng muốn:

- First-person view.
- Street-level view.
- Camera xoay quanh điểm chạm.
- Điều khiển mobile linh hoạt hơn.
- Chơi một tay dễ hơn.

Lý do: Công trình đẹp nhưng người chơi muốn khám phá nó từ bên trong.

### 7. Công Cụ Xây Dựng Nâng Cao

Người dùng có thể hưởng lợi từ:

- Undo/redo.
- Copy/paste cụm nhà.
- Brush đặt nhiều block.
- Mirror/symmetry.
- Tùy chọn khóa màu hoặc vật liệu.
- Preview trước khi đặt.

Lý do: Khi công trình lớn, thao tác từng block có thể chậm và dễ sai.

### 8. Có Động Lực Nhẹ Nhưng Không Áp Lực

Một số người chơi thấy game lặp lại. Tuy nhiên, cộng đồng Townscaper thường không muốn biến nó thành game quản lý tài nguyên. Giải pháp phù hợp là thêm mục tiêu mềm:

- Daily prompt.
- Theme challenge.
- Gallery cộng đồng.
- Achievement nhẹ.
- Bộ sưu tập postcard.
- Seed of the day.

Không nên thêm:

- Thuế.
- Điện nước.
- Dân số bắt buộc.
- Thua cuộc.
- Timer ép buộc.
- Monetization gây áp lực.

## Bài Học Thiết Kế Từ Townscaper

Townscaper thành công vì giữ trải nghiệm rất gọn:

- Một hành động chính: đặt/xóa block.
- Kết quả đẹp ngay.
- Hệ thống procedural làm việc khó thay người chơi.
- Không ép học luật.
- Không phạt sai lầm.
- Giao diện tối giản.

Nếu làm game tương tự, cần giữ lại cảm giác nhẹ này. Cải tiến nên nằm ở chiều sâu sáng tạo, camera, vật liệu, grid, sự sống và hiệu năng, không nên biến game thành mô phỏng quản lý phức tạp.

