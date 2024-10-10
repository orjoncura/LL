
let url:"http://localhost:5197";

export default function Constants() {
    return {
        ApplicationName: "Fluente",
        Version: "1.0.0.0",
        API: url,
        SwaggerUrl: url + "/swagger/v1/swagger.json",
    };
}
