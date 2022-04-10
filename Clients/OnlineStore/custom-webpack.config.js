const webpack = require("webpack");

module.exports = {
  plugins: [
    new webpack.DefinePlugin({
      $ENV: {
        clientRoot: JSON.stringify(process.env.clientRoot),
        idpAuthority: JSON.stringify(process.env.idpAuthority),
        clientId: JSON.stringify(process.env.clientId),
        clientSecret: JSON.stringify(process.env.clientSecret),
        catalogRoot: JSON.stringify(process.env.catalogRoot),
        ENVIRONMENT: JSON.stringify(process.env.ENVIRONMENT),
      },
    }),
  ],
};
