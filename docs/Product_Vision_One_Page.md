# Product Vision One Page

## Tên Tạm Thời

Chưa chốt.

Tên cần gợi cảm giác:

- Cozy.
- Town.
- Island.
- Garden.
- Harbor.
- Little world.

Tên cần tránh:

- Quá giống Townscaper.
- Dùng "scaper" nếu dễ bị xem là clone.
- Tên quá generic, khó tìm trên store.

## Elevator Pitch

Một game xây thị trấn thư giãn, nơi người chơi đặt từng block để tạo nên những thị trấn nhỏ có sự sống. Game giữ cảm giác đơn giản và đẹp ngay như một món đồ chơi kiến trúc, nhưng mở rộng bằng địa hình, grid có thể kiểm soát, cư dân ambient và khả năng bước vào thị trấn để quan sát từ góc nhìn gần.

## Product Statement

Người chơi không vào game để thắng, tối ưu tài nguyên hay xây nhanh. Người chơi vào game để chậm rãi đặt từng khối, quan sát thị trấn thay đổi, thử màu sắc, nghe âm thanh dễ chịu và cảm thấy một nơi chốn nhỏ đang dần hình thành.

## Core Fantasy

"Tôi đang tự tay tạo ra một thị trấn nhỏ yên bình, từng block một, rồi có thể ngắm nhìn hoặc bước vào bên trong nó."

## Người Chơi Mục Tiêu

- Người thích game thư giãn.
- Người thích Townscaper nhưng muốn nhiều quyền kiểm soát hơn.
- Người thích xây dựng sáng tạo nhưng không muốn city-builder phức tạp.
- Người chơi mobile muốn trải nghiệm ngắn 5-15 phút.
- Người thích chụp ảnh/chia sẻ công trình đẹp.

## Trụ Cột Sản Phẩm

### 1. Xây Dễ, Đẹp Ngay

Người chơi chỉ cần chạm/click để đặt block. Hệ thống procedural tự biến block thành nhà, mái, vòm, cầu, sân hoặc chi tiết phù hợp.

### 2. Thư Giãn Trước Tất Cả

Không thời gian, không thất bại, không tài nguyên bắt buộc, không ép nhiệm vụ. Mọi interaction phải nhẹ, rõ và có phản hồi dễ chịu.

### 3. Tự Do Nhưng Có Kiểm Soát

Người chơi có thể chọn màu, vật liệu, địa hình, loại grid hoặc seed để tạo thị trấn theo ý mình thay vì chỉ phụ thuộc vào một grid cố định.

### 4. Thị Trấn Có Sự Sống

Cư dân nhỏ, chim, đèn, thuyền và các chuyển động ambient làm thị trấn bớt trống vắng. Đây không phải simulation kinh tế, chỉ là lớp sống nhẹ.

### 5. Khám Phá Và Chia Sẻ

Người chơi có thể chụp ảnh, đổi góc nhìn, và về sau có thể đi dạo/street view để cảm nhận công trình từ bên trong.

## Điểm Khác Townscaper

Townscaper là một toy xây thị trấn cực kỳ tối giản trên grid organic.

Game của chúng ta khác ở:

- Có chiến lược terrain rõ hơn: nước, đất, cỏ, đường đá.
- Có lựa chọn grid hoặc seed có kiểm soát.
- Có cư dân/ambient life là một mục tiêu thiết kế chính.
- Có định hướng street view/walk view để khám phá công trình.
- Có architecture/code foundation tối ưu mobile từ đầu.
- Có asset/art strategy phù hợp khi không có artist riêng.

## MVP Cần Chứng Minh Điều Gì

MVP không cần chứng minh toàn bộ sản phẩm cuối. MVP chỉ cần chứng minh:

- Đặt/xóa block có cảm giác tốt.
- Procedural architecture tạo ra thị trấn đẹp, không chỉ cube stack.
- Camera mobile dễ dùng.
- Save/load và undo/redo đáng tin.
- Visual đủ khác Townscaper và đủ hấp dẫn để chụp screenshot.
- Performance ổn trên thiết bị tầm trung.

## MVP Không Cần Có

- Copy/paste cụm nhà.
- Brush xây hàng loạt.
- Online gallery.
- Daily challenge.
- Export 3D model.
- Nhiều theme.
- Street view hoàn chỉnh.
- Cư dân AI phức tạp.
- Quản lý tài nguyên.

## Chức Năng Bắt Buộc Cho Prototype Core

- Đặt block.
- Xóa block.
- Chọn màu cơ bản.
- Grid organic island bản đầu.
- Sinh wall/roof/tower cơ bản.
- Camera orbit/pan/zoom.
- Debug view cho cell, neighbor, rule.
- Data model đầu tiên cho `TownData`, `CellData`, `GridCoord`.

## Chức Năng Bắt Buộc Cho MVP

- Đặt/xóa block mượt.
- Procedural architecture đủ phong phú: nhà, mái, tháp, vòm, cầu, sân nhỏ.
- Undo/redo.
- Save/load nhiều town.
- Camera build view tốt trên mobile.
- Photo mode đơn giản.
- Settings cơ bản.
- Một terrain mở rộng tối thiểu nếu không làm phình scope.
- Performance/code rules được áp dụng thật.

## Nguyên Tắc Monetization

- Không ads bắt buộc.
- Không gacha.
- Không energy.
- Không tiền ảo gây áp lực.
- Ưu tiên free download + full unlock một lần.
- Cosmetic/theme pack chỉ nên thêm sau launch nếu core game đã được yêu thích.

## Non-Negotiables

- Game phải chơi offline được.
- Không mất save.
- Đặt block không được giật rõ.
- UI không được che trải nghiệm xây.
- Không thêm feature nếu feature đó phá nhịp thư giãn.
- Không dùng asset license không rõ.
- Không để visual quá giống Townscaper.

## Câu Hỏi Đánh Giá Mọi Quyết Định

Khi thêm bất kỳ feature, asset hoặc system nào, phải hỏi:

- Nó có làm game thư giãn hơn không?
- Nó có làm người chơi tạo thị trấn đẹp dễ hơn không?
- Nó có làm visual khác biệt và nhất quán hơn không?
- Nó có làm performance hoặc save/load rủi ro hơn không?
- Nó có thật sự cần cho MVP không?

