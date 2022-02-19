const webpack = require('webpack');

module.exports = {
  plugins: [
    new webpack.DefinePlugin({
      $ENV: {
        apiRoot: JSON.stringify(process.env.apiRoot),
        clientRoot: JSON.stringify(process.env.clientRoot),
        idpAuthority: JSON.stringify(process.env.idpAuthority),
        clientId: JSON.stringify(process.env.clientId),
        clientSecret: JSON.stringify(process.env.clientSecret),
        ENVIRONMENT: JSON.stringify(process.env.ENVIRONMENT),
      }
    })
  ]
};
