<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false"     %>
<%@ Import Namespace="System.Net.Mail" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">

    protected void btnMensaje_Click(object sender, EventArgs e)
    {    
        //Response.Write("IsGoogleCaptchaValid:" + IsGoogleCaptchaValid().ToString() );
        //Response.End();
        if (this.Page.IsValid && IsGoogleCaptchaValid() )
        {
            MailMessage eM = new MailMessage();
            eM.Priority = MailPriority.Normal;
            eM.IsBodyHtml = false;
            eM.Subject = "Contacto EMECI";
            eM.To.Add("info@emeci.com");
            eM.From = new MailAddress( "info@emeci.com");

            eM.Body = new string('*', 100)  + "\r\n";
            eM.Body += "Nombre:" + txtnombre.Value + "\r\n";
            eM.Body += "Correo:" + txtcorreo.Value + "\r\n";
            if(chkMedico.Checked)
                eM.Body += "M&eacute;dico Especialista:SI" + "\r\n";
            else
                eM.Body += "Paciente:SI" + "\r\n";

            eM.Body += "T&eacute;lefono:" + txtTelefono.Value + "\r\n";
            eM.Body += "Comentario:" + txtcomentario.Value + "\r\n";
            eM.Body += new string('*', 100);
            Boolean bnd=false;
            try
            {

                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationManager.AppSettings["Host"];
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                string m = ConfigurationManager.AppSettings["MailFrom"];
                string p = ConfigurationManager.AppSettings["passwordMail"];
                client.Credentials = new System.Net.NetworkCredential(m, p);


                client.Send(eM);


                bnd = true;
            }
            catch( Exception ex)
            {
                Response.Write(ex.Message);
            }
            if( bnd )
                Response.Redirect("exito.html");

        }

    }


    private Boolean IsGoogleCaptchaValid()
    {

        string userIP = Request.UserHostAddress;

        string privateKey = ConfigurationManager.AppSettings["ReCaptcha.PrivateKey"];

        string postData = string.Format("?secret={0}&remoteip={1}&response={2}",
                                     privateKey,
                                     userIP,
                                     Request.Form["g-recaptcha-response"]);



        // Create web request
        WebRequest request = WebRequest.Create("https://www.google.com/recaptcha/api/siteverify" + postData);
        request.Method = "POST";
        request.ContentType = "application/json; charset=utf-8";

        Stream dataStream = request.GetRequestStream();
        //dataStream.Write(postDataAsBytes, 0, postDataAsBytes.Length);
        dataStream.Close();

        // Get the response.
        WebResponse response = request.GetResponse();

        using (dataStream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(dataStream))
            {
                string responseFromServer = reader.ReadToEnd();

                if (!(responseFromServer.IndexOf( "success\": true")>0))
                    return false;
                else return true;
            }
        }


        /////////////////////////////////////////////////////////
        //    try
        //    {
        //    string  recaptchaResponse  = Request.Form["g-recaptcha-response"];
        //    if( ! String.IsNullOrEmpty(recaptchaResponse) )
        //    {
        //        Net.WebRequest request  = Net.WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Lc9UgITAAAAAGBf9TH0MrrTpbkQjUfPTB7wtNWQ&response=" + recaptchaResponse);
        //        request.Method = "POST";
        //        request.ContentType = "application/json; charset=utf-8";
        //       string  postData  = "";

        //        //'get a reference to the request-stream, and write the postData to it
        //        using IO.Stream s  = request.GetRequestStream()
        //        {
        //            using  new IO.StreamWriter(s) sw
        //            {  
        //                sw.Write(postData);
        //            }
        //        }
        //        //''get response-stream, and use a streamReader to read the content
        //        using IO.Stream s  = request.GetResponse().GetResponseStream()
        //        {
        //            using new IO.StreamReader(s) sr
        //            {  
        //                //'decode jsonData with javascript serializer
        //                var jsonData = sr.ReadToEnd()
        //                if (jsonData = "{" + "\r\n" + "  "\"success\"": true" + "\r\n" + "}" )
        //                    return true;

        //            }
        //        }
        //    }
        //    }
        //Catch ex As Exception
        //    'Dont show the error
        //End Try
        //return false;
    }

</script>

<!DOCTYPE html>
<html lang="en">
<head>

<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport" content="width=device-width, initial-scale=1">

<title>Expediente M&eacute;dico Electr&oacute;nico Cl&iacute;nico Internacional</title>

<!-- Bootstrap -->
<link rel="stylesheet" href="css/bootstrap.css">
<link rel="stylesheet" href="css/emeci.css">

<!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
<!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    
     <script src='https://www.google.com/recaptcha/api.js'></script>
  
</head>

<body>

<div id="inicio"></div>
<!-- Navigation -->
<nav id="mainNav" class="navbar navbar-default navbar-custom navbar-fixed-top">
    <div class="container">
        <!-- Brand and toggle get grouped for better mobile display -->
        <div class="navbar-header page-scroll">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                <span class="sr-only">Toggle navigation</span> Menu <i class="fa fa-bars"></i>
            </button>
            <a class="navbar-brand page-scroll" href="#page-top"><img src="images/logo-emeci.png" class="img-responsive" alt=""></a>
        </div>
        
        
        <!-- Topbar -->
        <div class="topbar hidden-xs">
            <ul class="loginbar pull-right list-inline top-links">						 
                <li><a href="https://www.emeci.com/consultamedico/medico/login"><img src="images/icono-acceso.png" /> Acceso a M&eacute;dicos</a></li>
                <li><a href="https://www.facebook.com/Expediente.EMECI"><img src="images/icono-facebook.png" /> EMECI</a></li>
                <li><a href="https://twitter.com/emecimx"><img src="images/icono-twitter.png" /> emecimx</a></li>	
            </ul>
        </div>
        <!-- End Topbar -->
        
 
        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav navbar-right">
                <li><a class="page-scroll" href="home.aspx#inicio">Inicio</a></li>
                <li><a class="page-scroll" href="membresias.aspx">Membresias</a></li>
                <li><a class="page-scroll" href="especialistas.html">Especialistas</a></li>
                <li><a class="page-scroll" href="tips.html">Tips</a></li>
                <li><a class="page-scroll btn btn-primary" href="https://www.emeci.com/expediente/loginExp.aspx">Acceso a Expediente</a></li>
            </ul>
        </div>
        <!-- /.navbar-collapse -->
    </div>
    <!-- /.container-fluid -->
</nav>


 <!-- Header -->
<header class="homepage">
    <div class="container">
        
       <div class="intro">
        <div class="row">
        
            <div class="col-md-6">
                <a href="#info-pacientes">
                    <div class="intro-pacientes">
                        <h2>Informaci&oacute;n para Pacientes</h2>
                        <p>Expediente en l&iacute;nea con todas las ventajas de salud, disponibilidad, confianza y econom&iacute;a que EMECI le ofrece.</p>
                        
                    </div>
                </a>
            </div>
            <div class="col-md-6">
                <a href="#info-medicos">
                    <div class="intro-medicos">
                        <h2>Informaci&oacute;n para M&eacute;dicos</h2>
                        <p>EMECI ayuda al m&eacute;dico a realizar su trabajo reduciendo esfuerzo y aumentando ampliamente la eficiencia.</p>
                    </div>
                </a>
            </div>
        
        </div>
        </div>
        
        
    </div>
</header>


<!-- Seccion de Informaci&oacute;n para Pacientes -->
<section id="info-pacientes">
  <div class="container">
    <div class="row">
      <div class="col-xs-12 text-center">
        <h1>INFORMACI&Oacute;N PARA PACIENTES</h1>
        <p class="intro">En la revoluci&oacute;n electr&oacute;nica de nuestro siglo, es indispensable contar con nuestro propio expediente m&eacute;dico electr&oacute;nico, por lo que, para lograr este objetivo…</p>
      </div>
    </div>
    <div class="col-sm-6">
      <div class="destacado-pacientes icono-seguridad">
        <h2>Seguridad y Disponibilidad</h2>
        <p>EMECI ha considerado todos los factores de seguridad, al colocar el expediente m&eacute;dico en el mundo del Internet, donde se mantendr&aacute; seguro ante desastres naturales que pongan en riesgo la p&eacute;rdida de expedientes impresos tradicionales y donde est&eacute; disponible a tus necesidades las 24 horas del d&iacute;a, los 365 d&iacute;as de año y en cualquier parte del mundo donde se tenga acceso a Internet. </p>
      </div>
      <div class="destacado-pacientes icono-economia">
        <h2>Econom&iacute;a</h2>
        <p>El costo anual del servicio es pr&aacute;cticamente la tercera parte de una consulta ordinaria y cuando viajes y necesite tu hijo atenci&oacute;n medica , ahorrar&aacute;s dinero en la repetici&oacute;n de estudios innecesarios y anular&aacute;s la posibilidad de provocarle dolor al realizarle un estudio m&eacute;dico invasivo ya elaborado en la ciudad de residencia. </p>
      </div>
        <div style="text-align:center">
            <a class="page-scroll btn btn-primary" href="/video.mp4">Ver Video</a>
        </div>
    </div>
    <div class="col-sm-6">
      <div class="destacado-pacientes icono-confianza">
        <h2>Confianza</h2>
        <p> EMECI ha sido diseñado para hacer uso de servidores seguros (SSL) de alta tecnolog&iacute;a y una enorme capacidad de almacenamiento y procesamiento de informaci&oacute;n, as&iacute; como el contar con la m&aacute;s avanzada tecnolog&iacute;a para generar respaldos de esta informaci&oacute;n en tiempo real, lo que hace pr&aacute;cticamente imposible la desaparici&oacute;n de datos almacenados, ofreciendo al paciente, adem&aacute;s, la ventaja de actualizar sus datos de su Expediente Cl&iacute;nico, consulta a consulta, si as&iacute; se desea. </p>
      </div>
      <div class="destacado-pacientes icono-confidencialidad">
        <h2>Confidencialidad</h2>
        <p>EMECI ha pensado en tu derecho de otorgar la informaci&oacute;n de tu expediente m&eacute;dico cl&iacute;nico solo a los m&eacute;dicos que tu elijas y para lo cual ha diseñado una serie de claves de acceso. </p>
      </div>
    </div>
  </div>
</section>
<!-- /#info-pacientes -->

<section id="info-medicos">

    <div class="mosaico">
    
        <div class="column-item">    
            <div class="espaciado">
            <h1>INFORMACI&Oacute;N PARA M&Eacute;DICOS</h1>
                  <p class="intro">EMECI ayuda al m&eacute;dico a realizar su trabajo reduciendo esfuerzo y aumentando ampliamente la eficiencia.</p>
                  <p><a href="#" class="btn btn-primary">Acceso M&eacute;dicos</a></p>
            </div>
        </div>
        <div class="column-item fondo-medicos-1"></div> 
    
    </div>
    
    <div class="mosaico alterno ">
        <div class="column-item fondo-medicos-2"></div> 
        <div class="column-item">    
            <div class="espaciado">
                   <p>  El Expediente M&eacute;dico Electr&oacute;nico Cl&iacute;nico Internacional es llenado en su mayor parte por el mismo paciente dejando al m&eacute;dico dejando al m&eacute;dico la opci&oacute;n de colocar la informaci&oacute;n m&eacute;dica &uacute;til que crea conveniente. Esto brinda seguridad y confianza al m&eacute;dico que usa el programa y reduce costos para el paciente en la repetici&oacute;n de estudios o procedimientos, agiliza la atenci&oacute;n m&eacute;dica e incrementa la consulta al ser solicitado por el paciente for&aacute;neo como: "MEDICO-PLUS"
    </p> 
             </div>
        </div>
        
    
    </div>
    
    <div class="destacado-para-medicos">
    
    
    <div class="container">
        <p class="intro text-center">Proporciona al M&eacute;dico ventajas econ&oacute;micas adicionales como:</p>
    
        <div class="row">
            <div class="col-md-5 col-md-push-1">
                <ul>
                    <li>Incrementar el N&uacute;mero de pacientes ya que cuenta con un servicio pedi&aacute;trico inusual.</li>
                    <li>Ser preferido para consulta pedi&aacute;trica por pacientes for&aacute;neos con tarjeta EMECI.</li>
                    <li>Retener a sus pacientes ya que evidentemente se muestra m&aacute;s actualizado.</li>
                </ul>
            </div>
            <div class="col-md-5 col-md-push-1">
                <ul>
                    <li>Ser&aacute; visto por el p&uacute;blico general como un Pediatra m&aacute;s Profesional</li>
                    <li>Al reducir el tiempo de consulta incrementa la posibilidad de ver m&aacute;s pacientes</li>
                     
                </ul>
            </div>
        </div>
    </div>
    
    </div>

<div class="foto-doble"></div>
 
</section>
<!-- /#info-medicos -->



<section id="promo-apps">
    <div class="container">
        <div class="row">
            <div class="col-md-6">
                <h1>LLEVA EMECI A DONDE VALLAS</h1>
                <p class="intro">Contamos con aplicaciones m&oacute;viles para los padres y para el m&eacute;dico que permiten acceder a su Expediente M&eacute;dico Electr&oacute;nico en cualquier momento.</p>
                
                <p>
                	<a href="http://www.apple.com" class="boton-de-tienda"><img src="images/logo-appstore.png" alt=""></a>
                    <a href="http://www.android.com" class="boton-de-tienda"><img src="images/logo-gplay.png" alt=""></a>
                </p>
            </div>
            <div class="col-md-6">
            	<img src="images/emeci-apps-phone.png" alt="" class="img-responsive">
            </div>
        </div>
    </div>
</section>
<!-- /#promo-apps -->

<section id="formulario-contacto">

	<div class="container">
        <div class="contenedor-de-formulario">
                           
                <div class="row">
                
                    <div class="col-md-8 col-md-push-2">
                    <h1 class="text-center">&iquest;Necesitas m&aacute;s informaci&oacute;n? </h1>
                <p class="intro text-center">Llena el siguiente formulario con tus comentarios o dudas, as&iacute; podremos contactarnos contigo y brindarte mayor informaci&oacute;n.</p>
                    
                    
                    <form id="Form1" method="post" runat="server" >
                    
                    
                        <div class="col-md-6">
                            <div class="form-group  ">
                                <label>Nombre Completo:</label>
                                <input type="text" name="txtnombre" class="form-control" id="txtnombre" runat="server" value="">
								
								<asp:RequiredFieldValidator id="tfvNombre" runat="server" ErrorMessage="Se Requiere Nombre" ControlToValidate="txtnombre"
												Display="Dynamic"></asp:RequiredFieldValidator>
												
                            </div>
                            <div class="form-group  ">
                                <input type="checkbox" name="chkMedico" id="chkMedico" runat="server" class="checkbox inline" /> <label for="es-medico">Soy m&eacute;dico especialista.</label>
                            </div>        
                            <div class="form-group  ">
                                <label>Tel&eacute;fono (incluir clave lada):</label>
                                <input name="txtTelefono" type="text" id="txtTelefono" size="40" runat="server" class="form-control">
								
								<asp:RequiredFieldValidator id="rfvTelefono" runat="server" ErrorMessage="Se Requiere No. Telefonico" ControlToValidate="txtTelefono"
												Display="Dynamic"></asp:RequiredFieldValidator></td>
												
                            </div>     
                            <div class="form-group  ">
                                <label>Correo electr&oacute;nico:</label>
                                <input name="txtcorreo" type="text" id="txtcorreo" size="50" runat="server" class="form-control" >
								
								<asp:RequiredFieldValidator id="rfvCorreo" runat="server" ErrorMessage="Se Requiere Correo" ControlToValidate="txtcorreo"
												Display="Dynamic"></asp:RequiredFieldValidator>
											<asp:RegularExpressionValidator id="revCorreo" runat="server" ErrorMessage="Correo Invalido" ControlToValidate="txtcorreo"
												Display="Dynamic" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>

												
                            </div>     
                                                  
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="exampleInputEmail1">Comentarios o Dudas:</label>
                                <textarea class="form-control" name="txtcomentario" id="txtcomentario" cols="45" rows="5" runat="server" style="height:150px"></textarea>
                            
								<asp:RequiredFieldValidator id="rfvComentarios" runat="server" ErrorMessage="Se Requiere Comentario" ControlToValidate="txtcomentario"
												Display="Dynamic"></asp:RequiredFieldValidator>
												
							</div>
                            <div class="demo-recapcha">
                                <div class="g-recaptcha" data-sitekey="6Lc9UgITAAAAACALBQtsf4BB11I2jRddQCBgcj2G"></div>
                            </div>
                            
                        </div>
                        
                        <div class="col-md-12">
                            <asp:Button id="btnMensaje" Text="Enviar Mensaje" class="btn btn-primary center-block" runat="server" OnClick="btnMensaje_Click"></asp:Button>
                      
                        
                        </div>
                        
                        
                     </form>	
                     
                     </div>
                 </div>  
                  
        </div>
    
    </div>

</section>




<footer id="page-footer">
  <div class="container">
    <div class="row">
      <div class="col-xs-6">
        <p><strong>Expediente M&eacute;dico Electr&oacute;nico Cl&iacute;nico Internacional</strong> <br>
        2016 &reg; Todos los Derechos Resevados </p>
         
      </div>
      <div class="col-xs-6">
        <ul class="accesos list-inline">
          <li><a href="#">Apps M&oacute;viles</a></li>
          <li><a href="#">Soporte</a></li>
          <li><a href="avisos-de-privacidad.html">Aviso de Privacidad</a></li>
        </ul>
      </div>
    </div>
  </div>
</footer>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) --> 
<script src="js/jquery-1.11.3.min.js"></script> 
<!-- Include all compiled plugins (below), or include individual files as needed --> 
<script src="js/bootstrap.js"></script>
  <script>
    $(window).scroll(function() {
		if ($(this).scrollTop() > 100){ // Set position from top to add class
			$('.navbar').addClass("shrink");
		} else {
			$('.navbar').removeClass("shrink");
		}
	});
    </script>
    
    
    
</body>
</html>