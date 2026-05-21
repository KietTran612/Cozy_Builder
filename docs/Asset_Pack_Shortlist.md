# Asset Pack Shortlist

## Mục Đích

Tài liệu này shortlist các asset pack có thể dùng cho prototype và production của game cozy procedural town builder. Mục tiêu không phải chọn asset đẹp nhất, mà chọn asset:

- Có license rõ.
- Hợp Unity + URP.
- Hợp mobile.
- Có style cozy/stylized.
- Có modularity đủ tốt cho procedural generation.
- Không quá giống Townscaper.
- Không làm performance rủi ro từ đầu.

## Quyết Định Sơ Bộ

Nên test import 2 pack đầu tiên:

1. **KayKit Medieval Builder Pack 1.0**  
   Vai trò: free/CC0 prototype candidate, dùng để kiểm tra grid, terrain, road/coast/water, tile-based logic.

2. **Terrific Modular Fantasy Village**  
   Vai trò: paid foundation candidate, dùng để kiểm tra modular building pieces, mesh/material optimization, khả năng làm visual foundation.

Backup nếu chưa mua pack paid:

3. **Medieval house modular v2.0 - lite - URP**  
   Vai trò: free Unity Asset Store fallback để test modular house pipeline trong URP.

## Candidate 1: KayKit Medieval Builder Pack 1.0

Source:

- https://opengameart.org/content/kaykit-medieval-builder-pack-10

Thông tin chính:

- License: CC0.
- Có hơn 200 stylised medieval scenery assets.
- Có building, wall, road tiles, water tiles, river/coast, scenery.
- Có 3 style placement: hexagonal, square, free placement.
- Có nhiều biome/color variation.
- Free for personal and commercial use, no attribution required.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 5 | CC0, rất tốt cho prototype và commercial. |
| Visual style | 3 | Stylized/cozy, nhưng hơi RTS/tile-game hơn là Townscaper-like town. |
| Modularity | 4 | Có road, coast, water, tiles, building, wall. |
| URP compatibility | 3 | Không phải Unity package chuyên URP; cần tự import/material setup. |
| Mobile performance | 4 | Low-poly, phù hợp mobile. |
| Procedural compatibility | 4 | Rất tốt để test grid/tile/terrain logic. |
| Ease of editing in Blender | 4 | CC0, dễ chỉnh sửa. |
| Difference from Townscaper | 4 | Khác rõ vì thiên về tile/RTS/hex. |

Tổng: 31/40.

Ưu điểm:

- Không rủi ro license.
- Không tốn tiền để test.
- Có water/coast/roads, hợp với nhu cầu terrain/grid.
- Có square/hex/free placement, hữu ích cho prototype grid.
- Có thể dùng làm placeholder/prototype ngay.

Nhược điểm:

- Không chắc đủ đẹp làm visual foundation cuối.
- Style có thể hơi game-board/RTS.
- Có thể thiếu các module wall/roof chi tiết cần cho procedural architecture kiểu nhà ghép từng cell.
- Cần tự setup material/prefab trong Unity nếu không dùng bản Unity-specific.

Quyết định:

- **Prototype Only / Foundation Candidate nhẹ**.
- Nên test import đầu tiên vì miễn phí, license sạch, giúp kiểm tra pipeline nhanh.

## Candidate 2: Terrific Modular Fantasy Village

Source:

- https://terrific3d.com/unity-assets/terrific-modular-fantasy-village/

Thông tin chính:

- Hơn 700 meshes.
- Hơn 800 prefabs.
- One material cho các mesh, có transparent/emissive variants.
- Thiết kế modular.
- Standard Unity shader, mô tả có thể convert giữa Built-in/URP/HDRP.
- Có colliders.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Cần mua/xác nhận Unity Asset Store EULA. |
| Visual style | 4 | Stylized low-poly, có tiềm năng cozy nếu chỉnh màu/lighting. |
| Modularity | 5 | Rất mạnh: 700+ meshes, modular construction. |
| URP compatibility | 4 | Có mô tả chuyển đổi Built-in/URP/HDRP, cần test import. |
| Mobile performance | 5 | One material là lợi thế lớn cho batching/instancing. |
| Procedural compatibility | 5 | Rất phù hợp để test wall/roof/props/module assembly. |
| Ease of editing in Blender | 4 | Nhiều mesh, có thể chỉnh nhưng cần quản lý tốt. |
| Difference from Townscaper | 3 | Cần art direction riêng để tránh medieval generic. |

Tổng: 34/40.

Ưu điểm:

- Candidate mạnh nhất cho visual/procedural foundation.
- One material rất hợp performance rules.
- Nhiều mesh/prefab giúp tạo variation.
- Modular rõ ràng, hợp procedural building.

Nhược điểm:

- Có thể phải mua trước khi test đầy đủ.
- Medieval fantasy có thể lệch cozy coastal town nếu không chỉnh palette/lighting.
- Quá nhiều asset có thể làm scope rối nếu không giới hạn subset.
- Cần xác nhận trực tiếp trong Unity URP.

Quyết định:

- **Foundation Candidate**.
- Nên là paid pack đầu tiên để test nếu ngân sách cho phép.

## Candidate 3: Medieval house modular v2.0 - lite - URP

Source:

- https://assetstore.unity.com/packages/3d/environments/fantasy/medieval-house-modular-v2-0-lite-urp-189718

Thông tin chính:

- Free.
- Standard Unity Asset Store EULA.
- URP package.
- Modular house keywords.
- File size khoảng 501 MB.
- Original Unity version 2019.4.16.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA, free. |
| Visual style | 3 | Cần xem kỹ trong Unity; có thể medieval hơn cozy. |
| Modularity | 4 | Tập trung modular house, hữu ích cho wall/roof test. |
| URP compatibility | 5 | Bản URP cụ thể. |
| Mobile performance | 2 | File 501 MB, cần kiểm tra material/texture/poly. |
| Procedural compatibility | 4 | Tốt để test house module pipeline. |
| Ease of editing in Blender | 3 | Chưa rõ mesh/pivot/scale. |
| Difference from Townscaper | 3 | Phụ thuộc art pass. |

Tổng: 28/40.

Ưu điểm:

- Free.
- URP-ready.
- Hợp test modular house generation.
- Tốt làm fallback nếu chưa mua asset paid.

Nhược điểm:

- File khá lớn cho một lite pack.
- Có thể không đủ nội dung terrain/coast/roads.
- Có thể không hợp visual cuối.

Quyết định:

- **Prototype Only / Backup Test Candidate**.
- Nên test nếu chưa thể mua Terrific Modular Fantasy Village.

## Candidate 4: Low Poly Village by Unco Games

Source:

- https://uncogames.itch.io/low-poly-village

Thông tin chính:

- 28 buildings.
- 41 trees.
- 80 rocks/stones.
- 99 props.
- FBX/prefabs included.
- Simple color/flat shaded.
- URP/HDRP/Built-in compatibility, materials may need adjustment.
- Intended for prototyping, stylized games, mobile projects, cozy/low-poly indie projects.
- Itch page says no generative AI was used.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 3 | Need confirm Unity Asset Store license/source before production. |
| Visual style | 4 | Cozy/low-poly, likely closer to our mood. |
| Modularity | 3 | Says modular/lightweight, but may be mostly premade buildings. |
| URP compatibility | 4 | URP compatible, may need material adjustment. |
| Mobile performance | 4 | Low-poly optimized, overall 148,777 poly. |
| Procedural compatibility | 3 | Good for props/environment, uncertain for cell-based architecture. |
| Ease of editing in Blender | 4 | FBX and flat-shaded likely easy. |
| Difference from Townscaper | 4 | Cozy rural look can differentiate. |

Tổng: 29/40.

Ưu điểm:

- Visual direction có vẻ cozy và mobile-friendly.
- Nhiều props/nature giúp làm thị trấn sống hơn.
- Có thể là secondary asset tốt.

Nhược điểm:

- Cần xác nhận license qua nguồn chính thức.
- Nếu chủ yếu là premade buildings, không đủ cho procedural wall/roof foundation.
- Có thể phù hợp decoration hơn là core procedural system.

Quyết định:

- **Secondary Asset Candidate**.
- Không test đầu tiên cho procedural foundation, nhưng nên giữ trong shortlist.

## Candidate 5: Low Poly Modular Village - LOWPOLY MEDIEVAL FANTASY SERIES

Source:

- https://assetstore.unity.com/packages/3d/environments/low-poly-modular-village-lowpoly-medieval-fantasy-series-258073

Thông tin chính:

- Unity Asset Store EULA.
- Price khoảng $59.99.
- Built-in và URP compatible.
- File size khoảng 737.2 MB.
- Original Unity version 2021.3.40.
- Keywords: modular building, medieval village, PBR, walls.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA. |
| Visual style | 3 | Low-poly medieval; cần xem có đủ cozy không. |
| Modularity | 4 | Modular village/walls keywords; cần test thật. |
| URP compatibility | 5 | URP compatible. |
| Mobile performance | 2 | File 737 MB, PBR có thể nặng cho mobile. |
| Procedural compatibility | 4 | Có tiềm năng nếu module/pivot tốt. |
| Ease of editing in Blender | 3 | Có thể nặng/nhiều asset. |
| Difference from Townscaper | 3 | Cần art direction riêng. |

Tổng: 28/40.

Ưu điểm:

- Modular village đúng nhu cầu.
- URP compatible.
- Có thể có wall/building pieces tốt.

Nhược điểm:

- File lớn.
- PBR có thể quá nặng hoặc lệch low-poly cozy.
- Giá cao hơn một số candidate.

Quyết định:

- **Foundation Candidate dự phòng**.
- Chưa test đầu tiên vì rủi ro size/performance cao hơn Terrific.

## Candidate 6: Modular Low Poly Medieval Village

Source:

- https://assetstore.unity.com/packages/3d/environments/fantasy/modular-low-poly-medieval-village-254530

Thông tin chính:

- Price khoảng $30.
- Unity Asset Store EULA.
- URP compatible.
- File size khoảng 269.6 MB.
- Original Unity version 2022.2.8.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA. |
| Visual style | 3 | Medieval low-poly; cần xem có cozy không. |
| Modularity | 3 | Tên modular nhưng page ít chi tiết. |
| URP compatibility | 5 | URP compatible. |
| Mobile performance | 3 | 269 MB vừa phải nhưng cần test material/poly. |
| Procedural compatibility | 3 | Chưa đủ thông tin về module/pivot. |
| Ease of editing in Blender | 3 | Chưa rõ. |
| Difference from Townscaper | 3 | Cần art direction riêng. |

Tổng: 27/40.

Ưu điểm:

- Giá vừa phải.
- URP compatible.
- File size không quá lớn so với một số pack khác.

Nhược điểm:

- Page ít technical detail.
- Không rõ one-material/modular depth.
- Không đủ mạnh để ưu tiên hơn Terrific/KayKit.

Quyết định:

- **Backup Candidate**.

## Candidate 7: Quaternius Buildings Pack

Source:

- https://quaternius.com/packs/buildings.html

Thông tin chính:

- 9 buildings.
- Formats: FBX, OBJ, Blend.
- License: CC0.
- Free for personal and commercial projects.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 5 | CC0. |
| Visual style | 3 | Low-poly, but small pack. |
| Modularity | 2 | Mostly buildings, not enough modular pieces. |
| URP compatibility | 3 | Generic model formats, need manual material setup. |
| Mobile performance | 4 | Likely lightweight. |
| Procedural compatibility | 2 | Good placeholder buildings, weak for procedural module foundation. |
| Ease of editing in Blender | 5 | Includes Blend. |
| Difference from Townscaper | 3 | Depends on use. |

Tổng: 27/40.

Ưu điểm:

- CC0, safe.
- Easy to edit.
- Useful for placeholder/prototype props.

Nhược điểm:

- Too small for main foundation.
- Not enough modularity.

Quyết định:

- **Supplemental Prototype Asset**.

## Candidate 8: DETAILED - Medieval Village

Source:

- https://assetstore.unity.com/packages/3d/environments/fantasy/detailed-medieval-village-156457

Thông tin chính:

- Price khoảng $129.99.
- File size khoảng 3.8 GB.
- Standard Unity Asset Store EULA.
- Medieval/fantasy modular village environment.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA. |
| Visual style | 3 | Detailed medieval, likely too heavy/serious. |
| Modularity | 4 | Modular environment. |
| URP compatibility | 2 | Need verify; older original Unity version. |
| Mobile performance | 1 | 3.8 GB too heavy for our mobile-first foundation. |
| Procedural compatibility | 3 | Could be good, but overkill. |
| Ease of editing in Blender | 2 | Too large/complex. |
| Difference from Townscaper | 3 | Different, but not necessarily cozy. |

Tổng: 22/40.

Quyết định:

- **Reject for MVP foundation**.
- Too heavy and expensive for our current scope.

## Candidate 9: Simple Town - Cartoon Assets

Source:

- https://assetstore.unity.com/packages/3d/environments/urban/simple-town-cartoon-assets-43500

Thông tin chính:

- Publisher: Synty Studios.
- Price khoảng $19.99.
- Standard Unity Asset Store EULA.
- File size khoảng 5.4 MB.
- Latest release date: Mar 12, 2025.
- Original Unity version: 2021.3.36.
- Related keywords include Mobile, Cute, City, Vehicle, Props, Building, Cartoon, Stylized.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA, nhưng license type Restricted Single Entity cần đọc kỹ. |
| Visual style | 5 | Cute/cartoon rõ, rất hợp yêu cầu mới. |
| Modularity | 3 | Có buildings/props/vehicles, nhưng chưa chắc đủ modular wall/roof cho procedural. |
| URP compatibility | 3 | Page không nêu rõ URP compatibility; cần test import/material. |
| Mobile performance | 5 | File nhỏ 5.4 MB, keyword Mobile, có khả năng rất nhẹ. |
| Procedural compatibility | 3 | Có thể tốt cho visual placeholder/building props, chưa chắc tốt cho cell-based architecture. |
| Ease of editing in Blender | 3 | Cần kiểm tra mesh/pivot sau import. |
| Difference from Townscaper | 5 | Cartoon/cute khác Townscaper rõ. |

Tổng: 31/40.

Ưu điểm:

- Cute/cartoon đúng hướng người dùng yêu cầu.
- File rất nhẹ.
- Publisher uy tín.
- Giá thấp.
- Có khả năng tốt cho visual mood prototype.

Nhược điểm:

- Có thể quá urban/modern so với cozy island town.
- Chưa rõ modularity đủ sâu cho procedural generation.
- Synty style khá nhận diện, cần chỉnh palette/lighting để tránh cảm giác asset-store generic.
- License type Restricted cần đọc kỹ trước production.

Quyết định:

- **Cute Visual Candidate / Secondary Foundation Candidate**.
- Nên test nếu muốn pivot art direction sang cute/cartoon rõ hơn.

## Candidate 10: Simple Buildings - Cartoon City

Source:

- https://assetstore.unity.com/packages/3d/environments/simple-buildings-cartoon-city-29003

Thông tin chính:

- Publisher: Synty Studios.
- Price khoảng $9.99.
- Standard Unity Asset Store EULA.
- File size khoảng 978.8 KB.
- Latest release date: Mar 5, 2025.
- Original Unity version: 2021.3.36.
- Related keywords include Mobile, Cute, Cartoon, Animation, color, Craft.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA, Restricted Single Entity cần đọc kỹ. |
| Visual style | 5 | Rất cute/cartoon, dễ đọc trên mobile. |
| Modularity | 2 | Có vẻ là building pack nhỏ, chưa chắc modular. |
| URP compatibility | 3 | Chưa rõ URP; cần test material conversion. |
| Mobile performance | 5 | File cực nhỏ, rất nhẹ. |
| Procedural compatibility | 2 | Tốt làm placeholder building, yếu cho procedural foundation. |
| Ease of editing in Blender | 3 | Cần kiểm tra mesh sau import. |
| Difference from Townscaper | 5 | Khác Townscaper rõ. |

Tổng: 29/40.

Ưu điểm:

- Rất nhẹ.
- Giá thấp.
- Cute/cartoon rõ.
- Tốt để kiểm tra visual mood nhanh.

Nhược điểm:

- Có thể không đủ modular cho core system.
- Có thể chỉ là premade buildings.
- Không đủ terrain/coast/water.

Quyết định:

- **Cute Prototype Visual Candidate**.
- Không nên dùng làm foundation procedural, nhưng đáng test để kiểm tra tone cute/kawaii.

## Candidate 11: STYLIZED Fantasy Village - Low Poly 3D Art

Source:

- https://assetstore.unity.com/packages/3d/environments/fantasy/stylized-fantasy-village-low-poly-3d-art-264834

Thông tin chính:

- Publisher: Daniel Mistage.
- Price khoảng $50.
- Standard Unity Asset Store EULA.
- File size khoảng 44.4 MB.
- Latest release date: Apr 13, 2025.
- Original Unity version: 2022.2.21.
- Built-in/URP/HDRP compatible on listed Unity versions.
- Related keywords include Cartoon, village, Medieval, town, House, Fantasy, Props, lowpoly, Stylized, Environment, Modular.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA. |
| Visual style | 4 | Stylized/cartoon village, có thể gần cozy hơn medieval packs nặng. |
| Modularity | 4 | Keywords có Modular; cần test depth thật. |
| URP compatibility | 5 | Page ghi URP compatible. |
| Mobile performance | 4 | File 44.4 MB, tương đối nhẹ. |
| Procedural compatibility | 4 | Có tiềm năng cho village modular foundation. |
| Ease of editing in Blender | 3 | Cần kiểm tra mesh/pivot. |
| Difference from Townscaper | 4 | Cartoon fantasy có thể khác nếu chỉnh palette. |

Tổng: 32/40.

Ưu điểm:

- URP compatible.
- File vừa phải.
- Cartoon/stylized village đúng hướng hơn pack medieval realistic.
- Có tiềm năng foundation nếu module/pivot tốt.

Nhược điểm:

- Cần mua.
- Chưa có đủ technical detail về số mesh/material.
- Fantasy/medieval vẫn cần chỉnh để thành cozy/kawaii.

Quyết định:

- **Cute/Stylized Foundation Candidate**.
- Đáng test cùng hoặc sau Terrific nếu muốn visual mềm/cute hơn.

## Candidate 12: Stylized Fantasy - Town Pack by Diminished Studios

Source:

- https://diminished-studios.com/news/stylized-fantasy-town-pack-released-on-unity-asset-store

Thông tin chính:

- Released Feb 24, 2026.
- Low-poly medieval-themed town pack.
- Includes 6 houses with 3 different textures, 1 church, 94 props, 10 terrain pieces, 3 floor pieces, 6 fences.
- URP compatible.
- Available on Unity Asset Store.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Likely Unity Asset Store EULA; cần mở Unity page để xác nhận khi mua. |
| Visual style | 4 | Stylized low-poly, có props/terrain giúp cozy. |
| Modularity | 3 | Có houses/props/terrain, nhưng chưa rõ wall/roof tách rời. |
| URP compatibility | 5 | Source ghi URP compatible. |
| Mobile performance | 4 | Low-poly, scope vừa phải. |
| Procedural compatibility | 3 | Tốt làm content/props, chưa chắc foundation procedural. |
| Ease of editing in Blender | 3 | Cần test. |
| Difference from Townscaper | 4 | Có thể khác nếu dùng terrain/props riêng. |

Tổng: 30/40.

Ưu điểm:

- Có terrain/floor/fences/props.
- URP compatible.
- Pack mới, scope vừa phải.
- Có thể bổ sung đời sống thị trấn.

Nhược điểm:

- Chỉ 6 houses, có thể ít variation cho procedural.
- Chưa rõ modularity sâu.
- Medieval-themed, cần chỉnh style để cute/cozy.

Quyết định:

- **Stylized Secondary Candidate**.
- Đáng xem nếu cần props/terrain cute hơn sau khi foundation được chọn.

## Candidate 13: Low Poly Mini Village Free

Source:

- https://assetstore.unity.com/packages/3d/environments/low-poly-mini-village-free-131677

Thông tin chính:

- Free.
- Standard Unity Asset Store EULA.
- File size khoảng 2.2 MB.
- Latest release date: Oct 29, 2018.
- Original Unity version: 2017.2.1.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA, free. |
| Visual style | 4 | Mini village có thể cute/toy-like. |
| Modularity | 2 | Cần kiểm tra, có thể chỉ là prefab nhỏ. |
| URP compatibility | 2 | Cũ, không rõ URP. |
| Mobile performance | 5 | File rất nhỏ. |
| Procedural compatibility | 2 | Có thể chỉ dùng placeholder. |
| Ease of editing in Blender | 3 | Cần test. |
| Difference from Townscaper | 4 | Mini/toy style khác rõ nếu hợp. |

Tổng: 26/40.

Ưu điểm:

- Free.
- Rất nhẹ.
- Có thể hữu ích để test tone mini/cute.

Nhược điểm:

- Asset cũ.
- Không rõ URP compatibility.
- Không rõ modularity.
- Không đủ làm foundation nếu thiếu module.

Quyết định:

- **Free Cute Placeholder Candidate**.
- Test nhanh nếu muốn so sánh mood, không ưu tiên foundation.

## Candidate 14: POLYGON - Town Pack - Art by Synty

Source:

- https://assetstore.unity.com/packages/3d/environments/urban/polygon-town-pack-art-by-synty-121115

Thông tin chính:

- Publisher: Synty Studios.
- Price khoảng $49.99, có sale tại thời điểm xem.
- Standard Unity Asset Store EULA.
- File size khoảng 104.6 MB.
- Latest release date: Mar 17, 2026.
- Original Unity version: 2022.3.56.
- Related keywords include Low Poly, Stylized, Cartoon, City, Building, Character, World.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA, Restricted Single Entity cần đọc kỹ. |
| Visual style | 4 | Cartoon/stylized polished, nhưng có thể urban hơn cozy. |
| Modularity | 4 | Synty town pack thường nhiều props/buildings, cần test module depth. |
| URP compatibility | 3 | Page không hiện URP table trong fetch; cần test/import. |
| Mobile performance | 4 | Low-poly, file 104.6 MB. |
| Procedural compatibility | 3 | Có thể tốt cho premade pieces/props, chưa chắc wall/roof cell system. |
| Ease of editing in Blender | 3 | Cần kiểm tra. |
| Difference from Townscaper | 5 | Khác rõ, cartoon/urban hơn. |

Tổng: 30/40.

Ưu điểm:

- Polished Synty style.
- Nhiều nội dung town/city.
- Tốt cho visual prototype nếu chọn hướng cartoon.

Nhược điểm:

- Có thể quá urban/modern.
- Synty style dễ bị nhận diện.
- Không chắc hợp procedural assembly từ cell.

Quyết định:

- **Cartoon Visual Candidate**.
- Nên cân nhắc nếu muốn style cartoon rõ, nhưng không test trước Terrific/KayKit nếu mục tiêu là procedural foundation.

## Candidate 15: Pandazole - City Town Lowpoly Pack

Source:

- https://assetstore.unity.com/packages/3d/props/exterior/pandazole-city-town-lowpoly-pack-205787

Thông tin chính:

- Free.
- Standard Unity Asset Store EULA.
- File size khoảng 4.0 MB.
- Latest release date: Dec 2, 2021.
- Original Unity version: 2020.3.23.
- Built-in/URP/HDRP compatible on listed Unity version.
- Related keywords include Cartoon, town, small town, Street, Trees, lowpoly, road, Building.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA, free. |
| Visual style | 4 | Cartoon/small town, có thể cute. |
| Modularity | 2 | Props/exterior, chưa rõ modular building. |
| URP compatibility | 5 | Page ghi URP compatible. |
| Mobile performance | 5 | File 4 MB, likely lightweight. |
| Procedural compatibility | 2 | Có thể tốt cho street props/placeholder, yếu cho procedural foundation. |
| Ease of editing in Blender | 3 | Cần test. |
| Difference from Townscaper | 5 | Khác rõ. |

Tổng: 30/40.

Ưu điểm:

- Free.
- URP compatible.
- Cartoon/small town.
- Rất nhẹ, tốt để test nhanh.

Nhược điểm:

- Không chắc có đủ wall/roof modules.
- Có thể chỉ là props/premade town elements.

Quyết định:

- **Free Cute/Cartoon Test Candidate**.
- Đáng test cùng KayKit vì miễn phí và URP-compatible.

## Candidate 16: Anime City Pack

Source:

- https://assetstore.unity.com/packages/3d/environments/anime-city-pack-199255

Thông tin chính:

- Price khoảng $34.99.
- Standard Unity Asset Store EULA.
- File size khoảng 895.9 MB.
- Latest release date: Mar 11, 2026.
- Original Unity version: 2022.3.48.
- Related keywords include mascot, Cute, town, district, anime, korea, Props, Modular Buildings, Stylized, urban, City, asian.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 4 | Unity Asset Store EULA. |
| Visual style | 5 | Anime/cute/kawaii direction rất rõ. |
| Modularity | 4 | Keywords có Modular Buildings. |
| URP compatibility | 3 | Cần xác nhận trực tiếp trong Unity page/import. |
| Mobile performance | 1 | File gần 896 MB, rủi ro cao cho mobile-first. |
| Procedural compatibility | 3 | Có thể modular nhưng scale/anime urban có thể khó hợp cell toy-town. |
| Ease of editing in Blender | 2 | Pack lớn, có thể nặng. |
| Difference from Townscaper | 5 | Rất khác. |

Tổng: 27/40.

Ưu điểm:

- Cute/anime/kawaii mạnh nhất trong shortlist.
- Có thể tạo khác biệt lớn nếu muốn chuyển sang anime town.

Nhược điểm:

- File quá lớn cho giai đoạn hiện tại.
- Urban/anime Korea có thể lệch cozy island/town builder.
- Performance và scope rủi ro.

Quyết định:

- **Kawaii Direction Reference / Not First Import**.
- Chỉ nên xem làm inspiration hoặc test sau nếu quyết định pivot mạnh sang anime/kawaii.

## Candidate 17: The Pastel Town

Source:

- https://assetstore.unity.com/packages/3d/environments/urban/the-pastel-town-182014

Reference found:

- https://unityassets4free.com/the-pastel-town/

Thông tin chính từ reference:

- Publisher: Tripolygon.
- 48 low-poly 3D models.
- Full demo scene.
- FBX and prefab files.
- Works in Unity 2019.4.12 and higher.
- Supports StandardRP and HDRP.
- URP available by converting materials.
- Built with UModeler; can be edited quickly if using UModeler.

Đánh giá:

| Nhóm | Điểm | Ghi chú |
| --- | ---: | --- |
| License | 3 | Cần dùng Unity Asset Store source chính thức, không dùng download reupload. |
| Visual style | 5 | Pastel/cute/toy-like rất hợp yêu cầu mới. |
| Modularity | 3 | 48 models, chưa rõ wall/roof module depth. |
| URP compatibility | 3 | URP qua material conversion, cần test. |
| Mobile performance | 4 | Low-poly, scope vừa. |
| Procedural compatibility | 3 | Có thể tốt cho visual direction, chưa chắc foundation. |
| Ease of editing in Blender | 4 | FBX/prefab; UModeler optional. |
| Difference from Townscaper | 5 | Pastel town khác rõ nếu art direction đúng. |

Tổng: 30/40.

Ưu điểm:

- Pastel/cute rất đúng hướng.
- Scope vừa phải.
- Có thể là visual reference tốt.

Nhược điểm:

- Cần xác nhận Unity Asset Store page/source chính thức trước khi mua.
- Không dùng bản free từ site reupload.
- URP cần conversion.
- Chưa rõ procedural modularity.

Quyết định:

- **Pastel/Kawaii Visual Candidate**.
- Đáng xem thêm, nhưng chỉ dùng nếu mua/tải từ Unity Asset Store chính thức.

## Recommended Test Import Order

### Test 1: KayKit Medieval Builder Pack 1.0

Reason:

- Free/CC0.
- Low risk.
- Good for grid, roads, water/coast, terrain.
- Helps validate import pipeline before spending money.

What to test:

- Import formats into Unity URP.
- Material setup.
- Scale.
- Tile/cell alignment.
- Coast/water/road procedural logic.
- Basic mobile FPS.

Expected use:

- Prototype terrain/grid placeholder.
- Not guaranteed final visual foundation.

### Test 2: Pandazole - City Town Lowpoly Pack

Reason:

- Free.
- URP compatible.
- Cartoon/small-town/cute direction.
- Very lightweight.
- Good quick comparison against KayKit's more tile/RTS look.

What to test:

- URP material status.
- Whether buildings/props read cute enough.
- Whether assets can align to cells.
- Whether roads/trees/props help town feel alive.
- If it can be used as secondary/placeholder content.

Expected use:

- Free cartoon/cute prototype candidate.
- Not guaranteed foundation.

### Test 3: Simple Town - Cartoon Assets or Simple Buildings - Cartoon City

Reason:

- Strong cute/cartoon direction.
- Lightweight Synty packs.
- Good for quickly testing whether a cartoon/kawaii visual direction feels better than medieval/fantasy.

What to test:

- URP conversion/material status.
- Whether the style is too urban/modern.
- Whether buildings are modular or only premade.
- Whether Synty style can be customized enough.

Expected use:

- Cute visual direction test.
- Likely secondary/prototype visual, not guaranteed procedural foundation.

### Test 4: Terrific Modular Fantasy Village

Reason:

- Strongest procedural foundation candidate.
- One material is very attractive for performance.
- Large modular mesh/prefab library.
- Best match for wall/roof/props procedural testing.

What to test:

- URP conversion.
- Material count and batching.
- Mesh pivots.
- Wall/roof/foundation modules.
- Can create 1-cell, 2-cell, stacked, courtyard, waterfront cases.
- Visual difference from Townscaper after lighting/palette adjustments.

Expected use:

- Potential main asset foundation.

### Backup Test: Medieval house modular v2.0 - lite - URP

Reason:

- Free.
- URP-specific.
- Good fallback to test modular house pieces.

Use only if:

- We cannot buy Terrific immediately.
- We need a free Unity Asset Store package to test URP/package workflow.

## Test Import Checklist

For each test asset pack, create a Unity scene and record:

- Import success/failure.
- URP material status.
- Number of materials.
- Number of textures.
- Average object scale.
- Pivot quality.
- Mesh modularity.
- Can build 1-cell house.
- Can build 2-cell row.
- Can stack 2-4 floors.
- Can create roof variants.
- Can create water edge/coast.
- Frame rate with 50/200/500 blocks.
- Draw calls/batches.
- GC Alloc during placement.
- Visual fit with cozy direction.

## Current Recommendation

Short term:

- Start with **KayKit Medieval Builder Pack 1.0** for free prototype import.
- Also test **Pandazole - City Town Lowpoly Pack** because it is free, URP-compatible, small, and more cartoon/cute.
- If we want a stronger cute/cartoon mood, test **Simple Town - Cartoon Assets** or **Simple Buildings - Cartoon City** next.
- If budget allows, buy/test **Terrific Modular Fantasy Village** as the strongest modular/procedural foundation candidate.
- Keep **The Pastel Town** and **Anime City Pack** as visual direction references, but do not import first due to URP/source/performance risks.

Do not start with huge/detailed/PBR packs. They may look impressive, but they create performance, scope and art-direction risk for a mobile-first cozy builder.
