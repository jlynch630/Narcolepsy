const path = require('path');
const isProduction = process.env.NODE_ENV == 'production';

const config = {
    entry: {
        index: {
            import: './src/index.ts',
            library: {
                type: 'module',
            },
        },
        graphqlWorker: {
            import: './src/monaco-graphql/src/graphql.worker.ts',

        }
    },
    output: {
        path: path.resolve(__dirname, '../wwwroot/script'),
        filename: "[name].js",
        publicPath: "/_content/Narcolepsy.GraphQL/script/"
    },
    module: {
        rules: [
            {
                test: /\.(ts|tsx)$/i,
                loader: 'ts-loader',
                exclude: ['/node_modules/'],
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader']
            },
            {
                test: /\.ttf$/,
                type: "asset/resource"
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2|png|jpg|gif)$/i,
                type: 'asset',
            },
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.jsx', '.js', '...'],
    },
    experiments: {
        outputModule: true
    }
};

module.exports = () => {
    if (isProduction) {
        config.mode = 'production';
    } else {
        config.mode = 'development';
    }
    return config;
};
