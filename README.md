
# UniSim - Modüler Otonom Lojistik ve Erişim Platformu Simülasyonu

Bu proje, "Ulaşan ve Erişen Türkiye 2053" vizyonu kapsamında tasarlanan modüler otonom lojistik platformlarının zorlu arazi ve hava koşullarında test edilebilmesi için geliştirilmiş 3 boyutlu bir fizik simülasyonudur. Kastamonu Üniversitesi Bilgisayar Mühendisliği "Proje 1" dersi kapsamında akademik amaçlarla geliştirilmiştir.

## ⚠️ Önemli Not (Depo İçeriği Hakkında)
Projenin 3D çevre modelleri, yüksek çözünürlüklü kaplamaları ve ağır ortam paketleri (ProfessionalAssets, TerrainSampleAssets vb.) boyut sınırları ve akademik odak nedeniyle bu repository'ye dahil edilmemiştir. 

Bu depo, projenin yazılım mimarisini, algoritma tasarımını ve temel C# kontrolcülerini değerlendirmek amacıyla oluşturulmuştur. Simülasyonun çekirdek mekaniklerini `Assets/Scripts` klasörü altından inceleyebilirsiniz.

<img width="1250" height="697" alt="Ekran görüntüsü 2026-06-08 074210" src="https://github.com/user-attachments/assets/47476eff-3994-443f-b623-dc2713dc5e2e" />
<img width="1253" height="691" alt="Ekran görüntüsü 2026-06-08 074116" src="https://github.com/user-attachments/assets/dd40ac3f-42e6-44ba-bed1-dfe024832362" />
<img width="1217" height="696" alt="Ekran görüntüsü 2026-06-08 074238" src="https://github.com/user-attachments/assets/0c228336-e666-45cd-a485-fb56399bd18f" />

## 🌟 Temel Özellikler
* **Modüler Araç Desteği:** İnsansız Hava Aracı (İHA), Kara Aracı ve Su Aracı gibi farklı otonom lojistik platformları arası dinamik geçiş.
* **Dinamik Model Yükleme:** Çalışma zamanında (runtime) dışarıdan `.obj` formatında 3D model alımı, otomatik boyutlandırma ve `MeshCollider` (çarpışma ağı) optimizasyonu.
* **Gerçek Zamanlı Çevre Faktörleri:** Rüzgar şiddeti, sis yoğunluğu ve hava sıcaklığının UI üzerinden değiştirilerek araçların fizik motoru (Rigidbody) üzerindeki anlık etkilerinin test edilmesi.
* **Veri Loglama (Telemetri):** Simülasyon sırasındaki aktif konum (X, Y, Z) ve hız verilerinin, algoritmik analizler için `.csv` formatında dışa aktarılması.
* **Gelişmiş Kamera Sistemi:** Zemin (Terrain) yüksekliğini anlık olarak hesaplayan ve arazi içine girmeyi engelleyen pürüzsüz dinamik kamera takibi.

## 🛠️ Kullanılan Teknolojiler
* **Oyun ve Fizik Motoru:** Unity 3D
* **Programlama Dili:** C# (.NET)
* **Mimari Yaklaşım:** Nesne Yönelimli Programlama (OOP), Single Responsibility Principle (SRP) ve modüler tasarım.

---
**Geliştirici:** Burak Dere
