<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="TIF.UI.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <section>
        <h3>Bienvenido a <%= NombreEmpresa %></h3>
        <p>
            En <%= NombreEmpresa %>, somos apasionados por la tecnología y dedicados a ofrecerte los mejores componentes de PC del mercado. 
            Desde nuestra fundación en 2010, nos esforzamos por ser tu tienda de confianza para todas tus necesidades de hardware, 
            ofreciendo una amplia gama de productos, desde procesadores y tarjetas gráficas de última generación hasta placas madre, memorias RAM, 
            sistemas de almacenamiento y periféricos.
        </p>
        <p>
            Nuestro equipo está formado por expertos entusiastas que no solo venden componentes, sino que también construyen, juegan y viven la tecnología cada día. 
            Estamos acá para asesorarte y ayudarte a armar la PC de tus sueños o actualizar tu equipo para que alcance su máximo potencial.
        </p>
        <p>
            Visitanos en nuestra sede central o explora nuestro catálogo online. ¡Vení a encontrar eso que te hace falta!
        </p>
    </section>

    <hr />

    <section>
        <h3>Nuestra Ubicación</h3>
        <p>Encontranos en:</p>
        <address>
            <%= NombreEmpresa %><br />
            Av. Caseros 3240<br />
            Parque Patricios, CABA<br />
            Argentina
        </address>
    
        <div id="mapContainer" style="width: 100%; max-width: 600px; height: 400px; margin-top: 20px; border: 1px solid #ccc;">
            <iframe 
                id="googleMap"
                width="100%" 
                height="100%" 
                frameborder="0" 
                style="border:0" 
                src="https://www.google.com/maps/embed?pb=!1m16!1m12!1m3!1d2760.3979392826654!2d-58.40924295541918!3d-34.637403055369504!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!2m1!1spolo%20tecnologico!5e0!3m2!1sen!2sar!4v1749524641076!5m2!1sen!2sar" 
                allowfullscreen=""
                aria-hidden="false"
                tabindex="0">
            </iframe>
        </div>
    
        <button type="button" onclick="showMyLocation()" style="margin-top: 15px; padding: 10px 15px; cursor: pointer;">Mostrar mi Ubicación en el Mapa</button>
    </section>
    </main>
    <script type="text/javascript">
        function showMyLocation() {
            console.log('Acá arranca la función');
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function(position) {
                    var lat = position.coords.latitude;
                    var lon = position.coords.longitude;
                    var mapFrame = document.getElementById('googleMap');
                    mapFrame.src = `https://maps.google.com/maps?q=${lat},${lon}&hl=es&z=14&output=embed`;
                    alert('Tu ubicación aproximada es: Latitud ' + lat + ', Longitud ' + lon + '. El mapa se ha actualizado.');
                }, function(error) {
                    handleLocationError(error);
                });
            } else {
                alert("La geolocalización no es soportada por este navegador.");
            }
        }

        function handleLocationError(error) {
            switch(error.code) {
                case error.PERMISSION_DENIED:
                    alert("El usuario denegó la solicitud de geolocalización.");
                    break;
                case error.POSITION_UNAVAILABLE:
                    alert("La información de ubicación no está disponible.");
                    break;
                case error.TIMEOUT:
                    alert("La solicitud para obtener la ubicación del usuario ha caducado.");
                    break;
                case error.UNKNOWN_ERROR:
                    alert("Un error desconocido ocurrió.");
                    break;
            }
        }
    </script>
</asp:Content>
