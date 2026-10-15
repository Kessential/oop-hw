# BÁO CÁO NHẬT KÝ SỬ DỤNG AI - TUẦN 01

- Tham số biến thể: K = 0 → Cả 10 bài đều làm theo biến thể K = 0.
- Mô hình AI đã sử dụng: **Claude Code (Claude Sonnet 5, Anthropic)** — KHÔNG sử dụng Google AI
  Studio / Gemini như tài liệu hướng dẫn thực hành gợi ý (lựa chọn cá nhân của sinh viên). Toàn bộ
  mã nguồn được sinh trực tiếp trong môi trường lập trình (không qua giao diện chat Google AI Studio),
  sau đó được biên dịch và chạy thử thật bằng `dotnet build` / `dotnet run` để đối chiếu với kết quả
  mong đợi trong ngân hàng đề bài trước khi đưa vào bản nộp.
- Môi trường biên dịch & chạy thử: **Visual Studio** (thay vì Visual Studio Code như tài liệu hướng
  dẫn gợi ý) — dùng để mở, biên dịch (`dotnet build`) và chạy (`dotnet run`) dự án. Visual Studio và
  VS Code đều chỉ là lớp giao diện bao quanh cùng .NET SDK/CLI, nên mã nguồn, cách biên dịch và kết
  quả chạy thử không đổi giữa hai công cụ; không ảnh hưởng tới tiêu chí "biên dịch thành công & chạy
  đúng" của rubric (mục 1, 40%). Thư mục cache `.vs/` do Visual Studio tự sinh đã được loại bỏ khỏi
  bản nộp, tương tự `bin/`/`obj/`.
- Nguồn đề bài: File "Bo_10_BaiTap_LapTrinh_ThamSoHoa.pdf" (Phần III - biến thể K = 0 cho từng bài)
  và file "Huong_Dan_Thuc_Hanh_Tuan_01.docx" (quy ước coding C#, cấu trúc dự án, Console Menu).

## 1. Nhật ký theo từng bài tập (Từ Bài 01 đến Bài 10)

### Chương trình chính: Program.cs (Entry Point & Console Menu)
- Prompt đã gửi: "Viết file `Program.cs` làm entry point của chương trình console C#. Trong hàm
  `static void Main(string[] args)`, thiết lập `Console.OutputEncoding`/`InputEncoding = Encoding.UTF8`
  để hiển thị tiếng Việt đúng trên Console Windows, sau đó xây dựng vòng lặp `do-while` với
  `switch-case` hiển thị menu từ Bài 01 đến Bài 10 (mỗi lựa chọn gọi hàm tĩnh `BaiNN.Chay()` tương
  ứng) và lựa chọn 0 để thoát chương trình. Đọc lựa chọn bằng `int.TryParse` để không bị crash khi
  người dùng nhập sai định dạng."
- Đánh giá phản biện: Đúng vai trò entry point chuẩn của .NET — không cần khai báo `StartupObject`
  trong `.csproj` vì đây là file `.cs` duy nhất trong dự án chứa `Main`. Vòng lặp `do-while` đảm bảo
  menu hiển thị lại sau mỗi lần chạy xong một bài, đúng yêu cầu "Console Menu Runner" trong tài liệu
  hướng dẫn thực hành (mục II.3, mẫu `Program.cs`).
- Lỗi phát hiện & Cách sửa: Bản đầu lo ngại `Console.ReadLine()` trả về `null` khi input rỗng/EOF sẽ
  làm `int.TryParse` ném lỗi — đã kiểm tra lại: `int.TryParse(string?, out int)` chấp nhận tham số
  `null` hợp lệ (chỉ trả về `false`), nên chương trình không crash, chỉ in "Lua chon khong hop le" và
  lặp lại menu. Không cần sửa thêm.
- Bộ test case đã kiểm thử: Chạy `dotnet run` với input giả lập qua stdin lần lượt `1 2 3 4 5 6 7 8 0`
  và `9 10 0` — chương trình hiển thị đúng menu, gọi đúng `BaiNN.Chay()` theo từng lựa chọn, quay lại
  menu sau mỗi bài, và thoát sạch (in "Tam biet!") khi chọn 0 — không treo, không crash.

### Bài 01: Tính cước điện sinh hoạt lũy tiến 5 bậc
- Prompt đã gửi: "Viết chương trình C# hướng cấu trúc tính tiền điện sinh hoạt theo 5 bậc lũy tiến
  (100/50/50/100 kWh đầu và phần còn lại, đơn giá lần lượt 550/900/1250/1450/1700 đ/kWh), cộng thêm
  VAT 10%. Viết hàm tĩnh `TinhTienDien(double kwh, out double thueVAT)` và `InHoaDon(double kwh)` in
  chi tiết từng bậc, tiền trước thuế, VAT và tổng tiền. Chạy thử với 3 test case: 75 kWh, 175 kWh,
  350 kWh."
- Đánh giá phản biện: Code sinh ra đúng theo phong cách hướng cấu trúc (static method, không dùng
  class/OOP thừa), dùng `double` xuyên suốt nên không bị lỗi chia lấy phần nguyên. Có tách vòng lặp
  qua mảng mức bậc/đơn giá thay vì if-else lồng nhau — vẫn đúng thuật toán và dễ đọc hơn if-else 5 cấp.
- Lỗi phát hiện & Cách sửa: Không phát hiện lỗi logic. Đã tự bổ sung kiểm tra `kwh <= 0` để tránh in
  hóa đơn với số liệu âm/vô nghĩa (đề bài gốc không yêu cầu rõ nhưng đây là input không hợp lệ).
- Bộ test case đã kiểm thử (dotnet build / dotnet run, chọn mục 1): 175 kWh → Tiền trước thuế =
  131.250 đ, VAT = 13.125 đ, Tổng = 144.375 đ — khớp đúng kết quả mẫu trong ngân hàng đề bài.

### Bài 02: Phân tích thừa số nguyên tố (dạng lũy thừa chuẩn)
- Prompt đã gửi: "Viết hàm tĩnh phân tích một số nguyên dương N thành thừa số nguyên tố dạng
  `p1^e1 * p2^e2 * ...` bằng chia thử tới `sqrt(N)`, đồng thời đếm số lượng ước nguyên tố phân biệt.
  Test với N = 360 (kỳ vọng `360 = 2^3 * 3^2 * 5^1`, đếm = 3), N = 17 (số nguyên tố), N = 1 (trường
  hợp biên không hợp lệ)."
- Đánh giá phản biện: Thuật toán chia thử dừng đúng ở điều kiện `p * p <= n` (độ phức tạp O(sqrt(N))
  theo đúng yêu cầu tối ưu trong đề), dùng kiểu `long` nên không tràn số với N lớn.
- Lỗi phát hiện & Cách sửa: Không phát hiện lỗi. Đã chủ động thêm test biên N = 1 để đảm bảo hàm
  không chạy sai/ crash khi N < 2 (đề yêu cầu N nguyên dương nhưng không loại trừ N = 1).
- Bộ test case đã kiểm thử: N=360 → `360 = 2^3 * 3^2 * 5^1`, số ước SNT = 3 (khớp mẫu); N=17 →
  `17^1`, đếm = 1; N=1 → báo "khong hop le" thay vì crash.

### Bài 03: Modul số nguyên tố & Giả thuyết Goldbach
- Prompt đã gửi: "Viết hàm tĩnh `IsPrime(int n)`, `ListPrimes(int m)` in các số nguyên tố <= m, và
  `Goldbach(int n, out int p, out int q)` tìm cặp số nguyên tố p, q sao cho p + q = n (n chẵn > 2).
  Test với M = 10 (in các SNT <= 10) và kiểm tra Goldbach cho N = 20."
- Đánh giá phản biện: Đúng logic giả thuyết Goldbach — duyệt a từ 2 đến n/2, dừng ngay khi tìm được
  cặp đầu tiên (không cần duyệt hết) nên hiệu quả.
- Lỗi phát hiện & Cách sửa: Bản đầu tiên không xử lý trường hợp n lẻ/n<=2 khi gọi Goldbach — đã bổ
  sung kiểm tra `n <= 2 || n % 2 != 0` trả về false ngay, tránh vòng lặp vô ích hoặc kết quả sai.
  Đã tự thêm test biên Goldbach(7) (số lẻ) để xác nhận nhánh này hoạt động đúng.
- Bộ test case đã kiểm thử: SNT <= 10: 2 3 5 7 (khớp mẫu); Goldbach(20) → `20 = 3 + 17` (khớp mẫu
  trong ngân hàng đề bài); Goldbach(7) → false, in thông báo không áp dụng.

### Bài 04: Mảng 1 chiều - Lọc và xóa số âm tận cùng bằng 6
- Prompt đã gửi: "Viết hàm tĩnh tìm phần tử âm đầu tiên có trị tuyệt đối tận cùng là 6, và hàm xóa
  toàn bộ số âm khỏi mảng ngay tại chỗ bằng kỹ thuật hai con trỏ (two-pointer), không dùng mảng phụ,
  độ phức tạp O(N) thời gian và O(1) bộ nhớ. Test với mảng [12, -16, -26, 45, -3, 8]."
- Đánh giá phản biện: Đúng yêu cầu ràng buộc kỹ thuật — không cấp phát `List`/mảng phụ nào, chỉ dùng
  2 chỉ số `writeIdx`/`readIdx` trên chính mảng gốc.
- Lỗi phát hiện & Cách sửa: Không phát hiện lỗi. Đã kiểm tra kỹ điều kiện `Math.Abs(a[i]) % 10 == 6`
  để chắc chắn chỉ bắt số âm tận cùng là 6 (không nhầm với số dương tận cùng 6).
- Bộ test case đã kiểm thử: mảng gốc [12, -16, -26, 45, -3, 8] → phần tử âm tận cùng 6 đầu tiên là
  -16 (vị trí 1); sau khi xóa còn [12, 45, 8] — khớp chính xác kết quả mẫu trong ngân hàng đề bài.

### Bài 05: Ma trận 2 chiều - Sắp xếp Zigzag
- Prompt đã gửi: "Viết hàm tĩnh sắp xếp toàn bộ ma trận vuông tăng dần theo kiểu zigzag: dồn toàn
  bộ phần tử vào một danh sách, sắp xếp tăng dần, rồi điền lại theo hàng — hàng chẵn (0-based) điền
  trái sang phải, hàng lẻ điền phải sang trái. Test với ma trận 3x3 [[9,2,7],[4,6,1],[8,3,5]]."
- Đánh giá phản biện: Cách dùng một danh sách phẳng đã sắp xếp rồi refill theo chiều xen kẽ là cách
  gọn và đúng thuật toán "trải phẳng - sort - đổ lại", khớp với gợi ý thuật toán trong đề (ứng dụng
  giống nén ảnh JPEG zigzag).
- Lỗi phát hiện & Cách sửa: Không phát hiện lỗi sau khi truy vết tay từng phần tử.
- Bộ test case đã kiểm thử: [[9,2,7],[4,6,1],[8,3,5]] → [[1,2,3],[6,5,4],[7,8,9]] — khớp mẫu.

### Bài 06: Struct Phân số - Đại số cơ bản
- Prompt đã gửi: "Định nghĩa `struct PhanSo { long TuSo; long MauSo; }`. Viết hàm tĩnh `RutGon` (rút
  gọn tối giản, chuẩn hóa mẫu số luôn dương, ném `DivideByZeroException` nếu mẫu số = 0), và 4 phép
  toán `Cong/Tru/Nhan/Chia`, cùng hàm `SoSanh` so sánh chính xác bằng nhân chéo (không dùng số thực
  để tránh sai số). Test với P1 = 2/4, P2 = -3/9."
- Đánh giá phản biện: Việc so sánh phân số bằng nhân chéo (`a.TuSo*b.MauSo` so với `b.TuSo*a.MauSo`)
  thay vì ép sang `double` là điểm cộng — tránh sai số dấu phẩy động, đúng tinh thần "đại số chính
  xác" của bài toán phân số.
- Lỗi phát hiện & Cách sửa: Bản đầu `Chia` chỉ kiểm tra mẫu số của phép chia mà quên chưa chặn tử số
  của số chia bằng 0 (chia cho phân số 0/n) — đã bổ sung kiểm tra `b.TuSo == 0` và ném
  `DivideByZeroException` kèm try/catch minh họa trong `Chay()`.
- Bộ test case đã kiểm thử: rút gọn P1 → 1/2, P2 → -1/3; Tổng=1/6, Hiệu=5/6, Tích=-1/6, Thương=-3/2,
  SoSanh(P1,P2)=1 (khớp mẫu: Tổng=1/6, Tích=-1/6); phép chia cho phân số mẫu=0 được bắt lỗi đúng.

### Bài 07: Danh sách liên kết đơn - CRUD & Tìm kiếm
- Prompt đã gửi: "Vì C# không có con trỏ thô, hãy dùng `class Node { int Data; Node? Next; }` (kiểu
  tham chiếu, tương đương con trỏ trong C/C++) để cài danh sách liên kết đơn. Viết hàm `PushBack`,
  `XoaTatCaGiaTri(ref Node? head, int x)` xóa mọi nút có giá trị x, `TimMax`, `TimSntDauTien`. Test
  với danh sách [10, 15, 7, 20, 15], xóa giá trị 15."
- Đánh giá phản biện: Dùng `class Node` là lựa chọn đúng và tối thiểu — không lạm dụng OOP nâng cao
  (không kế thừa/interface), chỉ là cấu trúc dữ liệu cần thiết để có ngữ nghĩa con trỏ trong C#.
- Lỗi phát hiện & Cách sửa: Bản đầu `XoaTatCaGiaTri` chỉ xóa các nút ở giữa/cuối mà bỏ sót trường hợp
  chính `head` cũng mang giá trị cần xóa (và có thể nhiều nút đầu liên tiếp cùng giá trị) — đã sửa
  bằng vòng `while (head != null && head.Data == x) head = head.Next;` trước khi duyệt tiếp.
- Bộ test case đã kiểm thử: DS = [10→15→7→20→15]; Max=20; SNT đầu tiên=7; sau khi xóa giá trị 15 →
  [10→7→20] — khớp chính xác kết quả mẫu trong ngân hàng đề bài.

### Bài 08: Đệ quy - Tháp Hà Nội cổ điển
- Prompt đã gửi: "Cài đệ quy Tháp Hà Nội cổ điển `ThapHaNoi(int n, char nguon, char dich, char
  trungGian, ref int demBuoc)`, in từng bước di chuyển đĩa và đếm tổng số bước qua tham số `ref`.
  Test với N = 3 đĩa, cọc nguồn A, cọc đích C, cọc trung gian B."
- Đánh giá phản biện: Đúng công thức đệ quy chia để trị chuẩn (chuyển N-1 đĩa sang cọc phụ, chuyển
  đĩa lớn nhất, rồi chuyển N-1 đĩa từ cọc phụ sang đích); dùng `ref int demBuoc` đúng như yêu cầu kỹ
  thuật truyền tham chiếu của tuần học.
- Lỗi phát hiện & Cách sửa: Không phát hiện lỗi; đã truy vết tay toàn bộ 7 bước đệ quy với N=3 để
  xác nhận số bước đúng bằng 2^3 - 1.
- Bộ test case đã kiểm thử: N=3 → 7 bước di chuyển đúng thứ tự, tổng số bước = 7 = 2^3 - 1 (khớp
  mẫu trong ngân hàng đề bài).

### Bài 09: File I/O - Đọc ma trận, ghi ma trận chuyển vị
- Prompt đã gửi: "Đọc ma trận vuông từ input.txt (dòng 1 là n, n dòng sau mỗi dòng n số nguyên cách
  nhau bởi dấu cách), kiểm tra tính hợp lệ (đúng n x n), ghi ma trận chuyển vị ra output.txt kèm
  timestamp. Dùng `StreamReader`/`StreamWriter` trong khối `using`, bọc try/catch với
  `FileNotFoundException`/`FormatException`/`Exception`. Tự sinh input.txt mẫu nếu chưa tồn tại."
- Đánh giá phản biện: Có kiểm tra chặt số lượng phần tử mỗi dòng đúng bằng n (nếu không thì ném
  `FormatException` báo "ma trận phải vuông") — đúng yêu cầu "kiểm tra hợp lệ dữ liệu" trong đề gốc,
  không chỉ đọc mù dữ liệu.
- Lỗi phát hiện & Cách sửa: Không phát hiện lỗi khi chạy thử; đã cố ý xóa `input.txt`/`output.txt`
  trước khi test để xác nhận nhánh "tự động tạo file mẫu" hoạt động đúng khi file chưa tồn tại.
- Bộ test case đã kiểm thử: input.txt tự sinh (3x3: 1 2 3 / 4 5 6 / 7 8 9) → output.txt chứa ma
  trận chuyển vị (1 4 7 / 2 5 8 / 3 6 9) kèm timestamp — khớp mẫu. File input.txt mẫu được nộp kèm.

### Bài 10: Xử lý chuỗi - Lọc trùng & sắp xếp dòng văn bản
- Prompt đã gửi: "Viết hàm chuẩn hóa khoảng trắng thừa (trim/collapse), hàm lọc các dòng trùng nhau
  sau khi chuẩn hóa và sắp xếp theo thứ tự từ điển, rồi in ra có đánh số thẳng cột (căn phải chữ số
  thứ tự). Test với danh sách [\"  banana  \", \"apple\", \"banana\", \" orange\"]."
- Đánh giá phản biện: Việc chuẩn hóa trước rồi mới đưa vào `HashSet` để lọc trùng là đúng thứ tự xử
  lý — nếu lọc trùng trước khi chuẩn hóa sẽ bị sót các dòng trùng nhau chỉ khác khoảng trắng.
- Lỗi phát hiện & Cách sửa: Không phát hiện lỗi. Đã kiểm tra kỹ việc đánh số dùng `PadLeft` theo độ
  rộng của tổng số dòng để cột số thẳng hàng ngay cả khi vượt quá 9 dòng (2 chữ số trở lên).
- Bộ test case đã kiểm thử: ["  banana  ", "apple", "banana", " orange"] → sau chuẩn hóa/lọc
  trùng/sắp xếp: `1. apple`, `2. banana`, `3. orange` — khớp mẫu.

## 2. Kết luận kiểm thử

Toàn bộ 10 bài đã được biên dịch thành công (`dotnet build` - 0 Warning, 0 Error) và chạy thử qua
Console Menu (`dotnet run`) với các test case nêu trên; kết quả thực tế khớp với kết quả mong đợi
trong ngân hàng đề bài (Phần III, mục 7 - Bộ Test Cases mẫu, biến thể K = 0) cho cả 10 bài. Không
phát hiện vi phạm quy tắc lập trình hướng cấu trúc (không dùng kế thừa/đa hình/interface ngoài
`class Node` tối thiểu cho danh sách liên kết), không có lỗi chia int/int, không có lỗi chia cho 0
chưa được xử lý.
