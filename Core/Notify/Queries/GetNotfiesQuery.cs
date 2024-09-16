﻿namespace Core.Pizza.Queries;

using Common.Entities;

public class GetNotifiesQuery : IRequest<ListResult<Notify>>
{
}

public class GetNotifiesQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetNotifiesQuery, ListResult<Notify>>
{
    public async Task<ListResult<Notify>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
    {
        //seed with intial 
        //databaseContext.Notifies.Add(new Notify
        //{
        //    CustomerId = 1,
        //    CustomerEmail = "kirantashnariansamy@gmail.com",
        //    DateSent =DateTime.UtcNow ,
        //    EmailContent = "<!DOCTYPE html>\r\n\r\n<html lang=\"en\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:v=\"urn:schemas-microsoft-com:vml\">\r\n<head>\r\n    <title></title>\r\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n    <meta content=\"width=device-width, initial-scale=1.0\" name=\"viewport\" />\r\n    <!--[if mso\r\n        ]><xml\r\n            ><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG /></o:OfficeDocumentSettings></xml\r\n    ><![endif]-->\r\n    <style>\r\n        * {\r\n            box-sizing: border-box;\r\n        }\r\n\r\n        body {\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n\r\n        a[x-apple-data-detectors] {\r\n            color: inherit !important;\r\n            text-decoration: inherit !important;\r\n        }\r\n\r\n        #MessageViewBody a {\r\n            color: inherit;\r\n            text-decoration: none;\r\n        }\r\n\r\n        p {\r\n            line-height: inherit;\r\n        }\r\n\r\n        .desktop_hide,\r\n        .desktop_hide table {\r\n            mso-hide: all;\r\n            display: none;\r\n            max-height: 0px;\r\n            overflow: hidden;\r\n        }\r\n\r\n        .image_block img + div {\r\n            display: none;\r\n        }\r\n\r\n        @media (max-width: 700px) {\r\n            .desktop_hide table.icons-inner {\r\n                display: inline-block !important;\r\n            }\r\n\r\n            .icons-inner {\r\n                text-align: center;\r\n            }\r\n\r\n                .icons-inner td {\r\n                    margin: 0 auto;\r\n                }\r\n\r\n            .fullMobileWidth,\r\n            .row-content {\r\n                width: 100% !important;\r\n            }\r\n\r\n            .mobile_hide {\r\n                display: none;\r\n            }\r\n\r\n            .stack .column {\r\n                width: 100%;\r\n                display: block;\r\n            }\r\n\r\n            .mobile_hide {\r\n                min-height: 0;\r\n                max-height: 0;\r\n                max-width: 0;\r\n                overflow: hidden;\r\n                font-size: 0px;\r\n            }\r\n\r\n            .desktop_hide,\r\n            .desktop_hide table {\r\n                display: table !important;\r\n                max-height: none !important;\r\n            }\r\n        }\r\n    </style>\r\n</head>\r\n<body style=\"background-color: #f9f9f9; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none\">\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"nl-container\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f9f9f9\" width=\"100%\">\r\n        <tbody>\r\n            <tr>\r\n                <td>\r\n                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-1\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td>\r\n                                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row-content stack\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #333; color: #000000; width: 680px\" width=\"680\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td class=\"column column-1\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: middle; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px\" width=\"100%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"image_block block-1\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt\" width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\" style=\"padding-bottom: 10px; padding-top: 10px; width: 100%; padding-right: 0px; padding-left: 0px\">\r\n                                                                <div align=\"center\" class=\"alignment\" style=\"line-height: 10px\">\r\n                                                                    <img alt=\"Pezza Logo\"\r\n                                                                         src=\"https://github.com/entelect-incubator/.NET/raw/master/Phase%207/pezza-logo.png\"\r\n                                                                         style=\"display: block; height: auto; border: 0; height: 150px; max-width: 100%\"\r\n                                                                         title=\"Pezza Logo\"\r\n                                                                         height=\"150\" />\r\n                                                                </div>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-2\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td>\r\n                                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row-content stack\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff; color: #000000; width: 680px\" width=\"680\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td class=\"column column-1\"\r\n                                                    style=\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tmso-table-lspace: 0pt;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tmso-table-rspace: 0pt;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tfont-weight: 400;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\ttext-align: left;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tborder-left: 1px solid #333333;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tborder-right: 1px solid #333333;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tpadding-bottom: 20px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tpadding-top: 20px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tvertical-align: top;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tborder-top: 0px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tborder-bottom: 0px;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\"\r\n                                                    width=\"100%\">\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block block-1\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word\" width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\" style=\"padding-bottom: 25px; padding-left: 20px; padding-right: 20px; padding-top: 10px\">\r\n                                                                <div style=\"font-family: Arial, sans-serif\">\r\n                                                                    <div class=\"\" style=\"font-size: 14px; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; mso-line-height-alt: 16.8px; color: #2f2f2f; line-height: 1.2\">\r\n                                                                        <p style=\"margin: 0; font-size: 14px; text-align: center; mso-line-height-alt: 16.8px\"><span style=\"font-size: 42px\">Order Received</span></p>\r\n                                                                    </div>\r\n                                                                </div>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"text_block block-2\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word\" width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\" style=\"padding-bottom: 10px; padding-left: 30px; padding-right: 30px; padding-top: 10px\">\r\n                                                                <div style=\"font-family: sans-serif\">\r\n                                                                    <div class=\"\" style=\"font-size: 14px; font-family: Arial, Helvetica Neue, Helvetica, sans-serif; mso-line-height-alt: 21px; color: #2f2f2f; line-height: 1.5\">\r\n                                                                        <p style=\"margin: 0; font-size: 14px; text-align: center; mso-line-height-alt: 24px\"><span style=\"font-size: 16px\">Hi %name%,</span></p>\r\n                                                                        <p style=\"margin: 0; font-size: 14px; text-align: center; mso-line-height-alt: 21px\"> </p>\r\n                                                                        <p style=\"margin: 0; mso-line-height-alt: 21px\"> </p>\r\n                                                                        <p style=\"margin: 0; text-align: center; mso-line-height-alt: 21px\">An order for your pizza has been placed, and it will arrive piping hot!</p>\r\n                                                                        <p style=\"margin: 0; text-align: center; mso-line-height-alt: 21px\"> </p>\r\n                                                                        <p style=\"margin: 0; text-align: center; mso-line-height-alt: 21px\">%pizzas%</p>\r\n                                                                    </div>\r\n                                                                </div>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-3\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td>\r\n                                    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row-content stack\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #333333; color: #000000; width: 680px\" width=\"680\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td class=\"column column-1\"\r\n                                                    style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 5px; padding-top: 5px; vertical-align: top; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px\"\r\n                                                    width=\"100%\">\r\n                                                    <table border=\"0\" cellpadding=\"10\" cellspacing=\"0\" class=\"text_block block-1\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word\" width=\"100%\">\r\n                                                        <tr>\r\n                                                            <td class=\"pad\">\r\n                                                                <div style=\"font-family: sans-serif\">\r\n                                                                    <div class=\"\" style=\"font-size: 14px; font-family: Arial, Helvetica Neue, Helvetica, sans-serif; mso-line-height-alt: 21px; color: #f9f9f9; line-height: 1.5\">\r\n                                                                        <p style=\"margin: 0; font-size: 12px; text-align: center; mso-line-height-alt: 21px\"> </p>\r\n                                                                    </div>\r\n                                                                </div>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                </td>\r\n            </tr>\r\n        </tbody>\r\n    </table>\r\n    <!-- End -->\r\n</body>\r\n</html>",
        //    Sent = false,
        //    Customer = new Customer
        //    {
        //        Id = 1,
        //        Name="Kiran Tash Nariansamy",
        //        Address="3 plane crescent waterways",
        //        Cellphone="065 979 8511",
        //        DateCreated= DateTime.Now,
        //        Email="kirannariansamy@gmail.com"
                
        //    },
        //});

        var result = await databaseContext.SaveChangesAsync(cancellationToken);
        var entities = databaseContext.Notifies
            .Select(x => x)
            .Include(x => x.Customer)
            .Where(x => x.Sent == false)
            .AsNoTracking();

        var count = entities.Count();
        var data = await entities.ToListAsync(cancellationToken);

        return ListResult<Notify>.Success(data, count);
    }
}