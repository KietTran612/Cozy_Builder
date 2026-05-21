# Kế Hoạch Làm App

## Mục Tiêu

Xây dựng một game thư giãn thuộc nhóm cozy procedural town builder: người chơi đặt block để tạo thị trấn đẹp, có sự sống, dễ chơi trên mobile, không áp lực và không biến thành game quản lý tài nguyên.

Mục tiêu quan trọng nhất không phải là xây nhanh, mà là mỗi thao tác xây đều dễ chịu, rõ ràng và tạo cảm giác thị trấn đang lớn lên một cách tự nhiên.

## Nguyên Tắc Phát Triển

- Ưu tiên cảm giác chơi trước số lượng chức năng.
- MVP phải chứng minh được vòng lặp đặt block, procedural sinh kiến trúc, camera và cảm giác thư giãn.
- Không đưa copy/paste, brush xây hàng loạt, công cụ creator nâng cao vào giai đoạn đầu.
- Undo/redo là công cụ quan trọng vì giúp người chơi không sợ sai.
- Mọi chức năng mới phải được kiểm tra theo câu hỏi: nó có làm game thư giãn hơn không?

## Đánh Giá Sau Rà Soát

Roadmap ban đầu đúng hướng, nhưng cần siết lại để dùng được như một kế hoạch sản xuất thật. Các điểm cần chỉnh:

- Cần có giai đoạn pre-production trước khi code gameplay chính, vì procedural grid, art direction và cảm giác đặt block là rủi ro nền tảng.
- MVP không nên ôm quá nhiều nội dung. MVP phải chứng minh core loop và cảm giác thư giãn, không phải chứng minh toàn bộ sản phẩm cuối.
- Street view là điểm khác biệt lớn so với Townscaper. Nếu dùng trong định vị marketing, nó không thể mãi là chức năng "nếu kịp"; cần có bản cơ bản trước beta/store-ready.
- Terrain, material và grid thứ hai rất quan trọng, nhưng phải đưa vào theo thứ tự. Nếu đưa tất cả vào MVP quá sớm sẽ làm procedural phình scope.
- Cần gate rõ sau từng giai đoạn: nếu core không đạt cảm giác đẹp/dễ chịu thì không nên mở rộng content.
- Cần performance budget, device matrix, save format version và debug tooling sớm hơn.

## Giai Đoạn 0: Pre-Production

Thời lượng đề xuất: 2-3 tuần.

Mục tiêu:

- Chốt phạm vi MVP.
- Chốt art direction đủ khác Townscaper.
- Xác định rủi ro kỹ thuật lớn nhất trước khi sản xuất.
- Chọn công cụ làm việc và nguồn tài nguyên phù hợp với thực tế không có artist riêng.

Việc cần làm:

- Viết one-page product vision.
- Chọn 1 art style chính: màu, tỷ lệ nhà, mái, nước, ánh sáng.
- Chọn engine/render pipeline, ưu tiên Unity URP nếu target chính là mobile.
- Viết bộ code rules cho Unity + URP để tối ưu performance ngay từ đầu.
- Chốt architecture/code rules: dùng VContainer ngay từ đầu, áp dụng DIP, struct cho data compact, UniTask cho async workflow.
- Lập danh sách nguồn asset có thể dùng: paid asset packs, CC0 assets, animation packs, texture/HDRI sources.
- Kiểm tra license của từng nguồn asset trước khi đưa vào project.
- Chọn 1-2 asset pack chính làm visual foundation, tránh trộn quá nhiều style.
- Tạo một scene test để import asset, kiểm tra scale, material, batching, mobile FPS và khả năng tách module cho procedural.
- Phác thảo input mobile: tap, hold, undo, rotate, zoom.
- Thiết kế dữ liệu cell/grid/save format bản đầu.
- Chọn target thiết bị tối thiểu.
- Tạo danh sách rule procedural bắt buộc cho prototype.

Tiêu chí đạt:

- Team có cùng định nghĩa về "game thư giãn".
- Có 5-10 hình tham chiếu visual.
- Có danh sách chức năng MVP và danh sách cắt bỏ rõ ràng.
- Có quyết định kỹ thuật ban đầu về grid, save data và mesh rebuild.
- Có asset shortlist kèm license, giá, style, pipeline và rủi ro.
- Có quyết định asset nào chỉ dùng prototype, asset nào có thể ship trong bản thương mại.
- Có tài liệu performance/code rules bắt buộc cho Unity + URP trước khi bắt đầu viết gameplay chính.
- Có tài liệu architecture/code rules cho VContainer, DIP, struct data và UniTask.

Gate quyết định:

- Nếu chưa chốt được art direction và core loop, chưa nên vào prototype dài.
- Nếu chưa tìm được nguồn asset đủ ổn hoặc license chưa rõ, chưa nên xây content thật trên asset đó.

## Giai Đoạn 1: Prototype Core

Thời lượng đề xuất: 3-5 tuần.

Mục tiêu:

- Chứng minh cảm giác đặt/xóa block.
- Có hệ thống grid cơ bản.
- Có procedural architecture tối thiểu.
- Camera mobile/desktop đủ dùng để xây.

Chức năng cần có:

- Đặt block.
- Xóa block.
- Chọn màu cơ bản.
- Grid organic island bản đầu.
- Sinh tường, mái, block cao tầng cơ bản.
- Camera orbit, pan, zoom.
- Âm thanh đặt block tạm thời.
- Save data tạm thời ở mức debug để kiểm tra cấu trúc dữ liệu.
- Debug view cho cell, neighbor và rule được chọn.
- Áp dụng code rules: không Instantiate/Destroy liên tục khi xây, không rebuild toàn bộ town nếu chỉ đổi một cell, không tạo material runtime tùy tiện.

Tiêu chí đạt:

- Người chơi có thể tạo một cụm nhà nhỏ trong 1-2 phút.
- Công trình nhìn có hình dáng thị trấn, không chỉ là cube xếp chồng.
- Đặt/xóa block có phản hồi tức thì.
- Rebuild chỉ tác động vùng nhỏ hoặc ít nhất có kế hoạch rõ để chuyển sang chunk rebuild.

Rủi ro:

- Procedural sinh hình không đẹp.
- Grid khó tương tác trên mobile.
- Camera gây khó chịu.
- Data model sai sẽ làm save/load, undo/redo và mở rộng grid khó về sau.

Ưu tiên xử lý:

- Cảm giác click/tap.
- Camera.
- Rule kiến trúc đơn giản nhưng ổn định.
- Dữ liệu cell/grid đủ sạch để không phải viết lại toàn bộ sau prototype.

Gate quyết định:

- Nếu đặt block chưa "đã tay" hoặc công trình chưa có nét thị trấn, không mở rộng terrain/material ở giai đoạn sau.

## Giai Đoạn 2: Vertical Slice

Thời lượng đề xuất: 5-8 tuần.

Mục tiêu:

- Tạo một lát cắt gameplay đủ đẹp để test với người chơi thật.
- Chứng minh game có tiềm năng hình ảnh và cảm xúc.

Chức năng cần có:

- Procedural roof tốt hơn.
- Cầu, vòm, cầu thang cơ bản.
- Đường ven nước/bến nước.
- Palette màu hoàn chỉnh bản đầu.
- Undo/redo.
- Save/load local.
- Photo mode đơn giản.
- Ánh sáng ngày/đêm hoặc time of day cơ bản.
- Một vài chi tiết ambient: chim, đèn cửa sổ, cây nhỏ.
- Art pass đầu tiên cho nước, mái, tường và outline/lighting.
- Performance budget bản đầu cho số lượng block mục tiêu.

Tiêu chí đạt:

- Một người chơi mới có thể tự tạo thị trấn đẹp mà không cần hướng dẫn dài.
- Có thể chụp screenshot đủ hấp dẫn để dùng trong store page/trailer.
- Undo/redo hoạt động ổn định.
- Game có ít nhất một hình ảnh đủ mạnh để làm key visual tạm thời.

Rủi ro:

- Quá nhiều rule procedural làm bug tăng.
- Visual chưa đủ khác biệt.
- Mobile performance giảm khi thị trấn lớn hơn.
- Nếu visual giống Townscaper quá gần, sản phẩm dễ bị xem là clone.

Ưu tiên xử lý:

- Visual clarity.
- Performance khi xây/xóa.
- Save/load không lỗi.
- Sự khác biệt visual và cảm giác sở hữu thị trấn.

Gate quyết định:

- Nếu screenshot không đủ hấp dẫn, chưa nên chuyển sang MVP nhiều chức năng.

## Giai Đoạn 3: MVP Playable

Thời lượng đề xuất: 8-12 tuần.

Mục tiêu:

- Có bản chơi được từ đầu đến cuối theo trải nghiệm cơ bản.
- Đủ ổn để test kín, lấy feedback và đo retention.

Chức năng bắt buộc:

- Đặt/xóa block mượt.
- Grid organic island.
- Procedural architecture đủ phong phú: nhà, mái, tháp, vòm, cầu, sân nhỏ.
- Undo/redo.
- Save/load nhiều town.
- Camera build view.
- Photo mode.
- Mobile gestures rõ ràng.
- Settings cơ bản: âm thanh, chất lượng đồ họa, reset camera.
- Một terrain mở rộng tối thiểu nếu không làm phình procedural, ví dụ cỏ/đất như một layer đơn giản.
- Monetization placeholder nếu chọn freemium, ví dụ mock full unlock/offline entitlement.

Chức năng nên có nếu kịp:

- Cư dân ambient đơn giản.
- Color wheel hoặc palette mở rộng.
- Grid thứ hai: canal hoặc square.

Chức năng để sau MVP:

- Copy/paste cụm nhà.
- Brush xây nhiều block.
- Street view hoàn chỉnh.
- Daily challenge.
- Gallery online.
- Export 3D model.
- Mod/custom palette.

Tiêu chí đạt:

- Người chơi test có thể chơi 10-20 phút không cần mục tiêu ép buộc.
- Ít nhất 70% tester hiểu cách xây trong 1 phút đầu.
- Screenshot/trailer nhìn khác biệt rõ với prototype thô.
- Không crash trong phiên chơi bình thường.
- Save/load và undo/redo không làm hỏng dữ liệu town.

Gate quyết định:

- Nếu retention trong test kín thấp vì người chơi chưa thấy "đẹp và thư giãn", ưu tiên polish core thay vì thêm content.

## Giai Đoạn 4: Alpha Test

Thời lượng đề xuất: 4-6 tuần.

Mục tiêu:

- Tìm lỗi, kiểm tra UX, đo xem game có đủ thư giãn và đáng quay lại không.

Việc cần làm:

- Test nội bộ trên nhiều máy Android.
- Test iOS nếu có target.
- Mời nhóm nhỏ 20-50 người chơi.
- Ghi nhận hành vi: thời lượng phiên, số town tạo, số lần dùng undo, điểm người chơi thoát.
- Thu feedback định tính: camera, cảm giác đặt block, màu sắc, sự sống trong thị trấn.
- Test save/load qua nhiều phiên.
- Test thiết bị yếu, tầm trung và cao.

Chỉ số nên theo dõi:

- Session length.
- Day 1 retention.
- Số block đặt trong phiên đầu.
- Tỷ lệ người dùng dùng save/load.
- Tỷ lệ người dùng chụp ảnh/photo mode.
- Lỗi crash theo thiết bị.
- FPS trung bình ở các mốc block: nhỏ, vừa, lớn.
- Số lần người chơi dùng undo sau thao tác sai.

Tiêu chí đạt:

- Người chơi mô tả game là thư giãn/dễ chịu.
- Ít phàn nàn về camera.
- Không có lỗi mất save.
- Performance ổn trên thiết bị tầm trung.
- Người chơi không cần đọc hướng dẫn dài để tạo town đầu tiên.

## Giai Đoạn 5: Beta / Store-Ready

Thời lượng đề xuất: 6-8 tuần.

Mục tiêu:

- Chuẩn bị bản có thể phát hành mềm.
- Tối ưu onboarding, performance, monetization và store assets.

Việc cần làm:

- Tutorial cực ngắn bằng thao tác, không dùng nhiều chữ.
- Polish âm thanh, haptic, hiệu ứng đặt block.
- Tối ưu chunk rebuild.
- Tối ưu memory.
- Thêm analytics cơ bản.
- Hoàn thiện privacy/data safety.
- Chuẩn bị trailer, screenshot, icon, store description.
- Tích hợp monetization đã chọn.
- Street view cơ bản nếu đây là thông điệp marketing chính.
- QA checklist cho gesture, save, purchase, restore purchase và offline play.

Tiêu chí đạt:

- Store page có hình ảnh thể hiện rõ game.
- Người chơi hiểu ngay đây là game xây thị trấn thư giãn.
- Bản build không có lỗi nghiêm trọng.
- Monetization không phá trải nghiệm.
- Các claim trên store page đều có trong build thật.

## Giai Đoạn 6: Soft Launch

Thời lượng đề xuất: 4-8 tuần.

Mục tiêu:

- Phát hành giới hạn để đo retention, conversion và phản hồi thật trước global launch.

Phạm vi:

- 1-3 thị trường nhỏ hoặc nhóm tester mở rộng.
- Android trước nếu muốn giảm độ phức tạp.
- Chưa chạy UA lớn.

Chỉ số cần xem:

- Conversion từ store page sang install.
- Day 1 retention.
- Day 7 retention.
- Tỷ lệ mua bản premium/IAP nếu dùng freemium.
- Review/comment về camera, giới hạn xây dựng, màu sắc, sự sống.
- Thiết bị nào crash hoặc tụt FPS.
- Tỷ lệ hoàn tất tutorial đầu.
- Tỷ lệ restore purchase lỗi nếu có IAP.

Quyết định sau soft launch:

- Nếu retention thấp: cải thiện onboarding, camera, cảm giác đặt block.
- Nếu người chơi thích nhưng không trả tiền: xem lại mô hình monetization và gói premium.
- Nếu người chơi chê thiếu nội dung: ưu tiên terrain, cư dân, grid thứ hai.
- Nếu người chơi không nhắc đến sự khác biệt so với Townscaper: ưu tiên street view, terrain/grid hoặc cư dân trước marketing lớn.

## Giai Đoạn 7: Global Launch

Thời lượng chuẩn bị: 3-4 tuần sau soft launch ổn định.

Mục tiêu:

- Phát hành chính thức, bắt đầu marketing đều đặn, thu review, cải tiến theo dữ liệu.

Việc cần làm:

- Chốt store page.
- Chốt trailer 30-45 giây.
- Chuẩn bị press kit.
- Đăng devlog ngắn.
- Liên hệ creator cozy game, mobile game, indie game.
- Theo dõi review hằng ngày trong 2 tuần đầu.
- Sửa lỗi nhanh.

## Kế Hoạch Sau Launch

Tháng 1 sau launch:

- Fix crash.
- Tối ưu thiết bị yếu.
- Cải thiện camera/gesture theo review.
- Thêm vài props hoặc màu miễn phí.

Tháng 2-3:

- Thêm grid mới.
- Thêm terrain/material mới.
- Thêm cư dân/thuyền nếu chưa có.
- Cải thiện photo mode.

Tháng 4-6:

- Theme pack đầu tiên.
- Daily prompt hoặc seed of the day.
- Gallery/share code nếu cộng đồng có nhu cầu.
- Xem xét copy/paste/brush nếu nhóm creator nâng cao thật sự cần.

## Ưu Tiên Kỹ Thuật

### Hệ Thống Dữ Liệu

- Lưu town theo cell/grid coordinate, height, material, color, terrain.
- Tách dữ liệu logical khỏi mesh render.
- Thiết kế save format có version để nâng cấp sau này.
- Hỗ trợ migration save giữa các version.
- Tách dữ liệu người chơi, entitlement premium và setting khỏi dữ liệu town.

### Procedural Generation

- Rule-based system cho từng module: wall, roof, bridge, stairs, courtyard, waterfront.
- Chỉ rebuild vùng bị ảnh hưởng thay vì toàn town.
- Có debug view để xem cell, neighbor, rule được chọn.
- Có test scene cho từng rule để tránh sửa rule này làm hỏng rule khác.

### Rendering

- Dùng mesh batching/instancing nếu phù hợp.
- LOD cho town lớn.
- Giới hạn shadow/lighting trên mobile.
- Tối ưu material count.
- Đặt performance budget rõ: FPS mục tiêu, memory mục tiêu, số block mục tiêu trên thiết bị tầm trung.
- Tuân thủ code rules Unity + URP: pooling, chunk rebuild, shared material, allocation-free update ở các path chạy thường xuyên.

### UX Mobile

- Tap để đặt/xóa.
- Two-finger pan/zoom/rotate.
- Camera xoay quanh điểm người chơi đang thao tác.
- Undo luôn dễ chạm.
- UI nhỏ gọn, không che town.
- Có tùy chọn đảo/tinh chỉnh gesture nếu tester phàn nàn.

### Art Và Content Pipeline

- Cần quy tắc tỷ lệ nhà, cửa, mái, đường, người dân để street view không bị sai scale.
- Nên có prefab/material naming convention từ đầu.
- Cần checklist để mỗi material mới hoạt động với roof, wall, bridge và waterfront.
- Không thêm theme mới nếu theme đó chưa làm rõ khác biệt gameplay hoặc visual.

### Offline-First

- Game nên chơi offline được.
- Save local là mặc định.
- Online gallery, cloud save hoặc daily prompt chỉ là mở rộng sau.
- Monetization phải xử lý được restore purchase và trạng thái offline rõ ràng.
