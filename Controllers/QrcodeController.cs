using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

[ApiController]
public class QRCodeController : ControllerBase
{
    [HttpGet]
    [Route("api/qrcode/{id}")]
    public IActionResult GenerateQRCode([FromRoute] string id)
    {
        string url = $"http://localhost:3000/ver-planta/{id}";

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);

        return File(qrCodeImage, "image/png");
    }
}