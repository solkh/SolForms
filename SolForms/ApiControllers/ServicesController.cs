namespace app.apicontrollers
{
    public static class fileservice
    {

        // [formiid]/submitions/[sessionid]/[filename].[ext]

        // year_m_d_guid


        private static readonly string filecenter = "/filecenter";
        public static string? saveSubmitionFile(byte[] data, string ext, Guid formid, Guid? sessionid = null)
        {
            if (data == null) return null;
            var filename = Guid.NewGuid().ToString();
            var fullphysicalpath = $"{filecenter}/{formid}/submitions/{(sessionid.HasValue ? "/" + sessionid : "")}/{filename}.{ext}";
            var token = $"s_{formid}_{sessionid}_{filename}.{ext}";
            File.WriteAllBytes(fullphysicalpath, data);
            return File.Exists(fullphysicalpath) ? token : null;
        }
        public static string? saveFormFile(byte[] data, string ext, Guid formid, Guid? questionid = null)
        {
            if (data == null) return null;
            var filename = Guid.NewGuid().ToString();
            var fullphysicalpath = $"{filecenter}/{formid}{(questionid.HasValue ? "/" + questionid : "")}/{filename}.{ext}";
            var token = $"s_{formid}_{questionid}_{filename}.{ext}";

            File.WriteAllBytes(fullphysicalpath, data);
            return File.Exists(fullphysicalpath) ? token : null;
        }

        // /filecenter/[formiid]/[questionid?]/[filename].[ext]
        // /filecenter/[formiid]/submitions/[sessionid]/[filename].[ext]
        // [formiid]_[questionid?]_[filename].[ext]
        // s_[formiid]_[sessionid]_[filename].[ext]
        //public string getfilepath(string token)
        //{
        //    var fullpath = _env.getphysicalpath(id);
        //    if (!System.IO.File.Exists(fullpath)) return notfound();
        //    var ext = fullpath.getextension(id)?.tolower() ?? "";
        //
        //    return fullpath;
        //}
        //public string getfiletumbnailpath(string id = "", int w = 1500, int h = 1500, bool crop = true)
        //{
        //    var physicalpath = _env.getphysicalpath(id);
        //    var ext = path.getextension(id)?.tolower() ?? "";
        //    if (!system.io.file.exists(physicalpath))
        //    {
        //        physicalpath = _env.webrootpath + "\\images\\default-image.jpg";
        //        ext = ".jpg";
        //    }
        //
        //    var thumbphysicalpath = _env.getphysicalpath(id.replace(ext, "")) + $"{w}x{h}{(crop ? "c" : "")}" + ext;
        //    var resizeoptions = new resizeoptions { size = new size(w, h), mode = crop ? resizemode.crop : resizemode.min };
        //
        //    if (!system.io.file.exists(thumbphysicalpath) && (ext == ".png" || ext == ".jpg" || ext == ".jpeg"))
        //    {
        //        using var image = image.load(physicalpath);
        //
        //        image.mutate(x => x.autoorient());
        //        image.mutate(x => x.resize(resizeoptions));
        //        //if (ext == ".png")
        //        //    image.save(thumbphysicalpath, new pngencoder());
        //        //else
        //        //    image.save(thumbphysicalpath, new jpegencoder());
        //        image.save(thumbphysicalpath, new webpencoder());
        //    }
        //
        //    if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
        //    {
        //        return new physicalfileresult(thumbphysicalpath, mimetypemap.getmimetype(".webp"));
        //    }
        //    return file($"/lib/file-icon-vectors/icons/vivid/{ext.substring(1)}.svg", "image/svg+xml");
        //}
    }
}